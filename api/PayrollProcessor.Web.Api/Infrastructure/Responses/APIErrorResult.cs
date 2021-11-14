using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Microsoft.AspNetCore.Mvc;

[DefaultStatusCode(500)]
public class APIErrorResult : JsonResult
{
    public APIErrorResult(string errorMessage = "") : base("")
    {
        string message = string.IsNullOrWhiteSpace(errorMessage)
            ? "Internal Server Error"
            : errorMessage;

        Value = new { Id = Guid.NewGuid(), Message = message };

        StatusCode = 500;
    }
}
