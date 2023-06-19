using MediatR;
using Assignment.Contracts.DTO;
using Assignment.Contracts.Data;
using Assignment.Core.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Assignment.Contracts.Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Nodes;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Assignment.Providers.Handlers.Queries
{
    [ExcludeFromCodeCoverage]
    public class SignInUserByUserNameQuery : IRequest<LoginResponseDTO>
    {
        public string UserName { get; }
        public string PassWord { get; }
        public SignInUserByUserNameQuery(string userName, string password)
        {
            UserName = userName;
            PassWord = password;
        }
    }

    [ExcludeFromCodeCoverage]
    public class SignInUserByUserNameQueryHandler : IRequestHandler<SignInUserByUserNameQuery, LoginResponseDTO>
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;////Ioption to be changes 
   
        public SignInUserByUserNameQueryHandler(IUnitOfWork repository, IMapper mapper, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _repository = repository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _configuration = configuration;         
        }

        public async Task<LoginResponseDTO> Handle(SignInUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            var user = await Task.FromResult(_repository.User.GetAll().Where(con => con.Username.Equals(request.UserName)).FirstOrDefault());
            if (user == null)
            {
                throw new EntityNotFoundException($"Invalid username or password!");
            }
            PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.PassWord);
            if (PasswordVerificationResult.Success != result)
            {
                throw new InvalidcredentialsException($"Invalid username or password!");
            }


            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Authentication:Jwt:Secret"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
             {
                    new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Name, request.UserName),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim(JwtRegisteredClaimNames.Jti,
                            Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };


            var tokenHandler = new JwtSecurityTokenHandler();
          
            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO res = new LoginResponseDTO()
            {
                token = tokenHandler.WriteToken(token),
                Role = user.Role,
                UserName = user.Username                
            };

            return token == null ? throw new EntityNotFoundException($"Faild to generate the token") : res;
        }     
    }
}