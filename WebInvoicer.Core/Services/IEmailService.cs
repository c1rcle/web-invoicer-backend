using System.Threading.Tasks;
using WebInvoicer.Core.Email;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Services
{
    public interface IEmailService
    {
        Task<TaskResult> SendForTaskResult(TaskResult<string> result, MessageData data);

        Task<TaskResult> SendForTaskResult(TaskResult result, MessageData data);

        Task SendEmail(EmailMessage message);
    }
}
