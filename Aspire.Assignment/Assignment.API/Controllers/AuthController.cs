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
        //private readonly JwtHandler _jwtHandler;
        //private readonly UserManager<User> _userManager;
        // UserManager<User> userManager
        public AuthController(IMediator mediator, IConfiguration configuration, IPasswordHasher<User> passwordHasher, IOptions<AppSettingsDTO> _applicationSettings, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            this._applicationSettings = _applicationSettings.Value;
            _logger = logger;
            //_jwtHandler = jwtHandler;
            //_userManager = userManager;
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
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { this._applicationSettings.Secret }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.idToken, new GoogleJsonWebSignature.ValidationSettings { Audience = new List<string> { this._applicationSettings.Secret } });

            var user = UserList.Where(x => x.Username == payload.Name).FirstOrDefault();

            return Ok(user);

        }
        //[HttpPost]
        //[Route("ExternalLogin")]
        //public async Task<IActionResult> ExternalLogin([FromBody] ExternalAuthDTO externalAuth)
        //{
        //    var payload = await _jwtHandler.VerifyGoogleToken(externalAuth);
        //    if (payload == null)
        //        return BadRequest("Invalid External Authentication.");

        //    var info = new UserLoginInfo(externalAuth.provider, payload.Subject, externalAuth.provider);

        //    var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        //    if (user == null)
        //    {
        //        user = await _userManager.FindByEmailAsync(payload.Email);
        //        if (user == null)
        //        {
        //            user = new User { Email = payload.Email, Username = payload.Email };
        //            await _userManager.CreateAsync(user);

        //            //prepare and send an email for the email confirmation

        //            await _userManager.AddToRoleAsync(user, "Viewer");
        //            await _userManager.AddLoginAsync(user, info);
        //        }
        //        else
        //        {
        //            await _userManager.AddLoginAsync(user, info);
        //        }
        //    }

        //    if (user == null)
        //        return BadRequest("Invalid External Authentication.");

        //    //check for the Locked out account

        //    var token = await _jwtHandler.GenerateToken(user);

        //    return Ok(new TokenResponse { Token = token, UserName = "" });
        //}



    }

    [ExcludeFromCodeCoverage]
    public class TokenResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string? Role { get; set; }
    }
    [ExcludeFromCodeCoverage]
    public class AuthenticateRequest
    {
        [Required]
        public string IdToken { get; set; }
    }
}
