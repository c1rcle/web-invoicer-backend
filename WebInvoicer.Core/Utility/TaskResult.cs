namespace WebInvoicer.Core.Utility
{
    public class TaskResult<T>
    {
        public bool Success { get; }

        public T Payload { get; }

        public string[] Errors { get; }

        public TaskResult(T payload)
        {
            Success = true;
            Payload = payload;
        }

        public TaskResult(string[] errors)
        {
            Success = false;
            Errors = errors;
        }
    }

    public class TaskResult
    {
        public bool Success { get; }

        public string[] Errors { get; }

        public TaskResult()
        {
            Success = true;
        }

        public TaskResult(string[] errors)
        {
            Success = false;
            Errors = errors;
        }
    }
}
