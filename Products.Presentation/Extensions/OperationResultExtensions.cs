using Products.Library.Contracts.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Products.Presentation.Extensions;

public static class OperationResultExtensions
{
    public static IActionResult ToActionResult<T>(
        this OperationResult<T> operationResult,
        ControllerBase controller)
    {
        if (operationResult.Success)
            return controller.Ok(operationResult.Result);

        var statusCode = operationResult.StatusCode ?? HttpStatusCode.BadRequest;

        var problemDetails = new ProblemDetails
        {
            Title = "Operation Failed",
            Detail = string.Join("; ", operationResult.Errors),
            Status = (int)statusCode
        };

        return statusCode switch
        {
            HttpStatusCode.BadRequest => controller.BadRequest(problemDetails),
            HttpStatusCode.NotFound => controller.NotFound(problemDetails),
            HttpStatusCode.InternalServerError => controller.StatusCode(StatusCodes.Status500InternalServerError, problemDetails),
            _ => controller.StatusCode((int)statusCode, problemDetails)
        };
    }
}
