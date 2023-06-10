using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Assignment.Contracts.DTO;
using Assignment.Core.Exceptions;
using Assignment.Core.Handlers.Commands;
using Assignment.Core.Handlers.Queries;
using Assignment.Providers.Handlers.Commands;
using Assignment.Providers.Handlers.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AppController> _logger;

        public UserController(IMediator mediator, IConfiguration configuration, ILogger<AppController> logger)
        {
            _mediator = mediator;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(BaseResponseDTO))]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Get()
        {
            var query = new GetAllUserQuery();
            var response = await _mediator.Send(query);
            if(response == null)
            {
                _logger.LogInformation("There no any data", DateTimeOffset.UtcNow);
                throw new NullReferenceException("There no any data!");
            }
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [ProducesErrorResponseType(typeof(BaseResponseDTO))]
        public async Task<IActionResult> Post([FromBody] CreateUserDTO model)
        {
            try
            {
                var command = new CreateUserCommand(model);
                int response = await _mediator.Send(command);

                if( response == 0)
                {
                    throw new NullReferenceException("Something went wrong!");
                }
                model.Id = response;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Authentication:Jwt:Secret"));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("userId", model.Username.ToString()) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                // Return the user and token in the response
                return Ok(new
                {
                    user = model,
                    token = tokenHandler.WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                return NotFound(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(UserDTO), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(BaseResponseDTO))]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            try
            {
                var query = new GetUserByUserNameQuery(userName);
                var response = await _mediator.Send(query);
                if(response == null)
                {
                    throw new EntityNotFoundException($"Not found this {userName}");
                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                return NotFound(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }

        [HttpGet]
        [Route("GetAllUserCountReport")]
        public async Task<IActionResult> GetUserReport()
        {
            var query = new GetAllUserReportQuery();
            var response = await _mediator.Send(query);
            
            return Ok(response);
        }
    }
}