using System;

namespace CSharpFunctionalExtensions;

public static class ResultExtensions
{
    public static TResult Match<T, TResult>(this Result<Maybe<T>> result, Func<Maybe<T>, TResult> onSome, Func<TResult> onNone, Func<string, TResult> onFailure)
    {
        if (result.IsFailure)
        {
            return onFailure.Invoke(result.Error);
        }

        var maybe = result.Value;

        return maybe.HasValue ? onSome.Invoke(maybe) : onNone.Invoke();
    }
}
