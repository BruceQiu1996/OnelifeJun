using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LiJunSpace.API.Dtos
{
    public static class ServiceResultExtension
    {
        public static ActionResult ToActionResult(this ServiceResult result)
        {
            ActionResult actionResult = result.StatusCode switch
            {
                HttpStatusCode.BadRequest => new BadRequestObjectResult(result.Message),
                HttpStatusCode.NotFound => new NotFoundObjectResult(result.Message),
                _ => new OkResult(),
            };

            return actionResult;
        }

        public static ActionResult ToActionResult<T>(this ServiceResult<T> result)
        {
            ActionResult actionResult = result.StatusCode switch
            {
                HttpStatusCode.BadRequest => new BadRequestObjectResult(result.Message),
                HttpStatusCode.NotFound => new NotFoundObjectResult(result.Message),
                _ => new OkObjectResult(result.Value),
            };

            return actionResult;
        }
    }
}
