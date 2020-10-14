using System.Threading.Tasks;
using WebInvoicer.Core.Dtos.User;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Services
{
    public interface IUserService
    {
        Task<ResultHandler> CreateUser(CreateUserDto data);

        Task<ResultHandler> ConfirmUser(ConfirmUserDto data);

        Task<ResultHandler> VerifyPassword(VerifyPasswordDto data);

        Task<ResultHandler> ResetPassword(string email);

        Task<ResultHandler> ChangePassword(PasswordDto data);

        Task<ResultHandler> ChangePassword(PasswordResetDto data);
    }
}
