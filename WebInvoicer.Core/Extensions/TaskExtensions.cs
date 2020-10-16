using System;
using System.Threading.Tasks;

namespace WebInvoicer.Core.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<TResult> NextAsync<TSource, TResult>(
            this Task<TSource> currentTask,
            Func<TSource, Task<TResult>> nextTask) => await nextTask(await currentTask);

        public static async Task<TResult> NextAsync<TSource, TResult, TData>(
            this Task<TSource> currentTask,
            Func<TSource, TData, Task<TResult>> nextTask,
            TData args) => await nextTask(await currentTask, args);
    }
}
