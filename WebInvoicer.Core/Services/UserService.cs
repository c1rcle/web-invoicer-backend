using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using WebInvoicer.Core.Dtos.User;
using WebInvoicer.Core.Extensions;
using WebInvoicer.Core.Repositories;
using WebInvoicer.Core.Token;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;

        private readonly IEmailService emailService;

        private readonly IMapper mapper;

        private readonly TokenConfiguration tokenConfig;

        public UserService(IUserRepository repository, IEmailService emailService, IMapper mapper,
            TokenConfiguration tokenConfig)
        {
            this.repository = repository;
            this.emailService = emailService;
            this.mapper = mapper;
            this.tokenConfig = tokenConfig;
        }

        public async Task<ResultHandler> CreateUser(CreateUserDto data)
        {
            var messageData = new MessageData(data.Email, MessageType.Confirmation);

            return ResultHandler.HandleTaskResult(await repository
                .CreateUser(data)
                .NextAsync(emailService.SendForTaskResult, messageData));
        }

        public async Task<ResultHandler> ConfirmUser(ConfirmUserDto data)
        {
            return ResultHandler.HandleTaskResult(await repository.ConfirmUser(data));
        }

        public async Task<ResultHandler> VerifyPassword(VerifyPasswordDto data)
        {
            var result = await repository.VerifyPassword(data);

            if (result.Success)
            {
                result.Payload = new
                {
                    Token = GenerateJwt(data),
                    User = mapper.Map<UserDataDto>(result.Payload)
                };
            }

            return ResultHandler.HandleTaskResult(result);
        }

        public async Task<ResultHandler> ResetPassword(string email)
        {
            var messageData = new MessageData(email, MessageType.PasswordReset);

            return ResultHandler.HandleTaskResult(await repository
                .ResetPassword(email)
                .NextAsync(emailService.SendForTaskResult, messageData));
        }

        public async Task<ResultHandler> ChangePassword(PasswordDto data)
        {
            var messageData = new MessageData(data.Email, MessageType.PasswordChange);

            return ResultHandler.HandleTaskResult(await repository
                .ChangePassword(data)
                .NextAsync(emailService.SendForTaskResult, messageData));
        }

        public async Task<ResultHandler> ChangePassword(PasswordResetDto data)
        {
            var messageData = new MessageData(data.Email, MessageType.PasswordChange);

            return ResultHandler.HandleTaskResult(await repository
                .ChangePassword(data)
                .NextAsync(emailService.SendForTaskResult, messageData));
        }

        private string GenerateJwt(VerifyPasswordDto data)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenConfig.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, data.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(tokenConfig.TokenExpiryTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
