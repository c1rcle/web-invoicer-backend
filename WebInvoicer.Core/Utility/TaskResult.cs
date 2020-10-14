namespace WebInvoicer.Core.Utility
{
    public class TaskResult
    {
        public bool Success { get; }

        public object Payload { get; set; }

        public TaskResult(bool success)
        {
            Success = success;
        }

        public TaskResult(bool success, object payload)
        {
            Success = success;
            Payload = payload;
        }
    }
}
