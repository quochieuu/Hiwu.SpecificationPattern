using Hiwu.SpecificationPattern.Core.Exceptions;
using Hiwu.SpecificationPattern.Core.Wrappers;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using static Hiwu.SpecificationPattern.Core.Common.Constants;

namespace Hiwu.SpecificationPattern.Core.Middlewares
{
    public class AppMiddleware
    {
        private readonly RequestDelegate _next;

        public AppMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new ResponseFailure();

                switch (ex)
                {
                    case ApiException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.ErrorCode = ErrorCodes.ResourceBadRequest;
                        responseModel.ErrorMessage = !string.IsNullOrEmpty(e.Message) ? e.Message : "Bad request exception";
                        break;

                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.ErrorCode = ErrorCodes.ResourceBadRequest;
                        responseModel.ErrorMessage = "Bad request exception";
                        break;

                    case AccountException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.ErrorCode = "userError";
                        responseModel.ErrorMessage = e.Message;
                        break;

                    case ForbiddenException e:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        responseModel.ErrorCode = "resourceForbidden";
                        responseModel.ErrorMessage = "Resource forbidden exception";
                        break;

                    case UnprocessableException e:
                        response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        responseModel.ErrorCode = "paramValidationFailed";
                        responseModel.ErrorMessage = "Some request parameters failed for validation.";
                        break;

                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.ErrorCode = ErrorCodes.ResourceInvalid;
                        responseModel.ErrorMessage = "Invalid resource is requested.";
                        break;

                    case TooManyRequestException e:
                        response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                        responseModel.ErrorCode = "rateLimitExceeded";
                        responseModel.ErrorMessage = "Too many attempts";
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.ErrorCode = "serverError";
                        responseModel.ErrorMessage = "Server cannot fulfill this request due to some server exception";
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel);

                await response.WriteAsync(result);
            }
        }
    }
}
