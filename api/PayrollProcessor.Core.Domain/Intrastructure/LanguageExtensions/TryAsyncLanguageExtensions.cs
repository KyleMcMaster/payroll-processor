using System;

namespace LanguageExt;

public static class TryAsyncLanguageExtensions
{
    /// <summary>
    /// Performs the given action if the operation results in a failed state
    /// </summary>
    /// <param name="t">Current operation</param>
    /// <param name="ifFail">Action to execute with the exception that caused the failure</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static TryAsync<T> DoIfFail<T>(this TryAsync<T> t, Action<Exception> ifFail) => async () =>
      {
          var optionResult = await t.Try();

          if (optionResult.IsFaulted)
          {
              optionResult.IfFail(ifFail);
          }

          return optionResult;
      };
}
