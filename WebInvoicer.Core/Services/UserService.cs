using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using WebInvoicer.Core.Dtos.User;
using WebInvoicer.Core.Repositories;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;

        private readonly IEmailService emailService;

        private readonly IMapper mapper;

        private readonly TokenData tokenData;

        public UserService(IUserRepository repository, IMapper mapper, TokenData tokenData)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.tokenData = tokenData;
        }

        public async Task<ResultHandler> CreateUser(CreateUserDto data)
        {
            //TODO send confirmation email.
            return await ResultHandler.HandleRepositoryCall(repository.CreateUser, data);
        }

        public async Task<ResultHandler> ConfirmUser(ConfirmUserDto data)
        {
            return await ResultHandler.HandleRepositoryCall(repository.ConfirmUser, data);
        }

        public async Task<ResultHandler> VerifyPassword(VerifyPasswordDto data)
        {
            var result = await ResultHandler.HandleRepositoryCall(repository.VerifyPassword, data);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                result.Payload = new
                {
                    Token = GenerateJwt(data),
                    User = mapper.Map<UserDataDto>(result.Payload)
                };
            }

            return result;
        }

        public async Task<ResultHandler> ResetPassword(string email)
        {
            //TODO send password reset email.
            return await ResultHandler.HandleRepositoryCall(repository.ResetPassword, email);
        }

        public async Task<ResultHandler> ChangePassword(PasswordDto data)
        {
            //TODO send password change notification email.
            return await ResultHandler.HandleRepositoryCall(repository.ChangePassword, data);
        }

        public async Task<ResultHandler> ChangePassword(PasswordResetDto data)
        {
            //TODO send password change notification email.
            return await ResultHandler.HandleRepositoryCall(repository.ChangePassword, data);
        }

        private string GenerateJwt(VerifyPasswordDto data)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenData.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, data.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(tokenData.TokenExpiryTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
