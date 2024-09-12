using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using System.Net;

namespace UserApi
{
    public static class Utilities
    {
        public static bool HasErrors(List<IdentityError> identityErrors, out ActionResult errorResponse)
        {
            if (identityErrors.Count > 0)
            {
                var errors = identityErrors.Select(e => e.Description).ToArray();
                errorResponse = new BadRequestObjectResult(new ResponseError
                {
                    StatusCode = $"{(int)HttpStatusCode.BadRequest}",
                    Messages = errors
                });
                return true;
            }

            errorResponse = null;
            return false;
        }
    }
}
