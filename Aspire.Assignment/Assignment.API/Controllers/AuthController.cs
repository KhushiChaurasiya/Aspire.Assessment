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
using Assignment.API;
using Assignment.Providers.Handlers.Commands;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using Google.Apis.Util;
using Newtonsoft.Json.Linq;

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
        //[Authorize]
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
            LoginResponseDTO res;
            var _jwtSettings = _configuration.GetSection("Jwt");
            var _goolgeSettings = _configuration.GetSection("Authentication:Google");
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _goolgeSettings.GetSection("client_id").Value }
            };

            var googleUser = await GoogleJsonWebSignature.ValidateAsync(externalAuth.idToken, new GoogleJsonWebSignature.ValidationSettings()
            {
                Clock = new clock(),
                Audience = new[] { _goolgeSettings.GetSection("client_id").Value }
            });


            //var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.idToken, settings);

            var name = googleUser.Name;
            //if (googleUser != null)
            //{
                CreateUserDTO createUserDTO = new CreateUserDTO()
                {
                    Firstname = googleUser.GivenName,
                    Lastname = googleUser.FamilyName,
                    Email = googleUser.Email,
                    IdToken = externalAuth.idToken,
                    Username = googleUser.Name,
                    Provider = externalAuth.provider,
                    Role = "User",
                    Password = "test@1996",
                    Address = "test",
                };

                var command = new CreateUserCommand(createUserDTO);
                int response = await _mediator.Send(command);


                //if (response == 0)
                //{
                //    throw new NullReferenceException("Something went wrong!");
                //}
                ////model.Id = response;

                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Authentication:Jwt:Secret"));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                 {
                    new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Name, googleUser.Name),
                        new Claim(ClaimTypes.Role, createUserDTO.Role),
                        new Claim(JwtRegisteredClaimNames.Jti,
                            Guid.NewGuid().ToString())
                }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };


                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);

                res = new LoginResponseDTO()
                {
                    token = tokenHandler.WriteToken(token),
                    Role = createUserDTO.Role,
                    UserName = googleUser.Name
                };

            // Return the user and token in the response
            return Ok(new
            {
                user = res,
                token = tokenHandler.WriteToken(token)
            });

        }
       
        //[HttpPost]
        //public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDTO externalAuth)
        //{
        //    try
        //    {
        //        var _jwtSettings = _configuration.GetSection("Jwt");
        //        var _goolgeSettings = _configuration.GetSection("Authentication:Google");
        //        var settings = new GoogleJsonWebSignature.ValidationSettings()
        //        {
        //            Audience = new List<string>() { _goolgeSettings.GetSection("client_id").Value }
        //        };

        //        var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.idToken, settings);

        //        return payload;
        //    }
        //    catch (Exception ex)
        //    {
        //        //log an exception
        //        return null;
        //    }
        //}


    }

    [ExcludeFromCodeCoverage]
    public class TokenResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
    [ExcludeFromCodeCoverage]
    public class AuthenticateRequest
    {
        [Required]
        public string IdToken { get; set; }
    }

    public class clock : IClock
    {
        public DateTime Now => DateTime.Now.AddMinutes(5);

        public DateTime UtcNow => DateTime.UtcNow.AddMinutes(5);
    }
}
