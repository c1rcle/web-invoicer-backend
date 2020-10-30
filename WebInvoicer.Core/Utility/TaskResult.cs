namespace WebInvoicer.Core.Utility
{
    public class TaskResult<T>
    {
        public bool Success { get; }

        public T Payload { get; }

        public TaskErrorType ErrorType { get; } = TaskErrorType.None;

        public string[] Errors { get; }

        public TaskResult(T payload)
        {
            Success = true;
            Payload = payload;
        }

        public TaskResult(TaskErrorType type)
        {
            Success = false;
            ErrorType = type;
        }

        public TaskResult(string[] errors, TaskErrorType type = TaskErrorType.Unprocessable)
        {
            Success = false;
            ErrorType = type;
            Errors = errors;
        }
    }

    public class TaskResult
    {
        public bool Success { get; }

        public TaskErrorType ErrorType { get; } = TaskErrorType.None;

        public string[] Errors { get; }

        public TaskResult()
        {
            Success = true;
        }

        public TaskResult(TaskErrorType type)
        {
            Success = false;
            ErrorType = type;
        }

        public TaskResult(string[] errors, TaskErrorType type = TaskErrorType.Unprocessable)
        {
            Success = false;
            ErrorType = type;
            Errors = errors;
        }
    }
}
