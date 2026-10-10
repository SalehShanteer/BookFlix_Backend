using BookFlix.Core.Services.Validation;
using BookFlix.Web.Dtos;
using Microsoft.AspNetCore.Mvc;
using static BookFlix.Core.Enums.GeneralEnums;

namespace BookFlix.Web.Controllers
{
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public abstract class ApiController : ControllerBase
    {
        protected IActionResult HandleFailure(Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Cannot handle a successful result as a failure.");
            }
            return result.Error.ErrorType switch
            {
                ErrorType.NotFound => NotFound(new ApiErrorResponse(result.Error.Key)),
                ErrorType.Validation => BadRequest(new ApiErrorResponse(result.Error.Key)),
                ErrorType.Conflict => Conflict(new ApiErrorResponse(result.Error.Key)),
                ErrorType.Unauthorized => Unauthorized(new ApiErrorResponse(result.Error.Key)),
                ErrorType.Forbidden => StatusCode(StatusCodes.Status403Forbidden, new ApiErrorResponse(result.Error.Key)),
                ErrorType.Failure => StatusCode(500, new ApiErrorResponse(result.Error.Key)),
                _ => StatusCode(500, new ApiErrorResponse("An unexpected error occurred."))
            };
        }
    }
}
