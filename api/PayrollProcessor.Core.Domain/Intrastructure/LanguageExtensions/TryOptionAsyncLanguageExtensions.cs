using System;
using LanguageExt;

namespace LanguageExt
{
    public static class TryOptionAsyncLanguageExtensions
    {
        /// <summary>
        /// Performs the given action if the operation results in a failed state
        /// </summary>
        /// <param name="t">Current operation</param>
        /// <param name="ifFail">Action to execute with the exception that caused the failure</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TryOptionAsync<T> DoIfFail<T>(this TryOptionAsync<T> t, Action<Exception> ifFail) => async () =>
           {
               var optionResult = await t.Try();

               if (optionResult.IsFaulted)
               {
                   optionResult.IfFail(ifFail);
               }

               return optionResult;
           };

        /// <summary>
        /// Performs (at most) one of the given actions if the operation results in a None value or a failed state
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ifNone">Action to execute if the operation results in no value</param>
        /// <param name="ifFail">Action to execute if the operation fails, with the exception that caused the failure</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TryOptionAsync<T> DoIfNoneOrFail<T>(this TryOptionAsync<T> t, Action ifNone, Action<Exception> ifFail) => async () =>
        {
            var optionResult = await t.Try();

            if (optionResult.IsFaulted)
            {
                optionResult.IfFail(ifFail);
            }
            else if (optionResult.IsNone)
            {
                optionResult.IfFailOrNone(ifNone);
            }

            return optionResult;
        };
    }
}
