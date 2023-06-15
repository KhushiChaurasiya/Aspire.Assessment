using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Assignment.Contracts.Data.Entities;
using Assignment.Contracts.DTO;
using Assignment.Core.Exceptions;
using Assignment.Providers.Handlers.Queries;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AppSettingsDTO _applicationSettings;
        private static List<User> UserList = new List<User>();
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, IConfiguration configuration, IPasswordHasher<User> passwordHasher, IOptions<AppSettingsDTO> _applicationSettings, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            this._applicationSettings = _applicationSettings.Value;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(BaseResponseDTO))]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var query = new GetAllUserQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [ExcludeFromCodeCoverage]
        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [ProducesErrorResponseType(typeof(BaseResponseDTO))]
        public async Task<IActionResult> Post([FromBody] UserDTO model)
        {
            try
            {
                var command = new SignInUserByUserNameQuery(model.Username, model.Password);
                var response = await _mediator.Send(command);

                var tokenModel = new TokenResponse() { Token = response.token, UserName = model.Username, Role = response.Role};

                // Return the user and token in the response
                return Ok(tokenModel);
            }
            catch (InvalidRequestBodyException ex)
            {
                return BadRequest(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = ex.Errors
                });
            }
        }

        
        [HttpGet]
        [Route("{name}")]
        [ProducesResponseType(typeof(UserDTO), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(BaseResponseDTO))]
        public async Task<IActionResult> GetById(string name)
        {
            try
            {
                var query = new GetUserByUserNameQuery(name);
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = new string[] { ex.Message }
                });
            }
        }

        [HttpPost]
        [ExcludeFromCodeCoverage]
        [Route("ExternalLogin")]
        [ProducesResponseType(typeof(ExternalAuthDTO), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(BaseResponseDTO))]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalAuthDTO externalAuth)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { this._applicationSettings.Secret }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);

            var user = UserList.Where(x => x.Username == payload.Name).FirstOrDefault();

            //if (user != null)
            //{
            //    return Ok(JWTGenerator(user));
            //}
            //else
            //{
            //    return BadRequest();
            //}

            return Ok(user);

        }

    }


    public class TokenResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string? Role { get; set; }
    }
    public class AuthenticateRequest
    {
        [Required]
        public string IdToken { get; set; }
    }
}
