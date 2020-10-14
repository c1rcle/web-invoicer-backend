using System;
using System.Threading.Tasks;

namespace WebInvoicer.Core.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<TResult> NextAsync<TSource, TResult>(
            this Task<TSource> inner,
            Func<TSource, Task<TResult>> outer) => await outer(await inner);

        public static async Task<TResult> NextAsync<TSource, TResult, TData>(
            this Task<TSource> inner,
            Func<TSource, TData, Task<TResult>> outer,
            TData args) => await outer(await inner, args);
    }
}
