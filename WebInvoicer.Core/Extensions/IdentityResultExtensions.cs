using System.Linq;
using Microsoft.AspNetCore.Identity;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Extensions
{
    public static class IdentityResultExtensions
    {
        public static TaskResult GetTaskResult(this IdentityResult result)
        {
            return result.Succeeded
                ? new TaskResult()
                : new TaskResult(result.GetErrorDescriptions());
        }

        public static string[] GetErrorDescriptions(this IdentityResult result)
        {
            return result.Errors.Select(x => x.Description).ToArray();
        }
    }
}
