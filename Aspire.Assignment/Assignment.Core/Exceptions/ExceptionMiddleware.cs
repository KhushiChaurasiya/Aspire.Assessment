using Assignment.Contracts.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Core.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (AccessViolationException avEx)
            {
                _logger.LogError($"Something went wrong: {avEx}");
                await HandleExceptionAsync(httpContext, avEx);
            }
            catch (UnauthorizedAccessException avEx)
            {
                _logger.LogError($"Invalid username and password: {avEx}");
                await HandleExceptionAsync(httpContext, avEx);
            }
            catch(ArgumentOutOfRangeException avEx) 
            {
                _logger.LogError($"Somthing went wrongs: {avEx}");
                await HandleExceptionAsync(httpContext, avEx);
            }
            catch(ArgumentNullException avEx) {
                _logger.LogError($"value is null: {avEx}");
                await HandleExceptionAsync(httpContext, avEx);
            }
            catch(EntityNotFoundException avEx)
            {
                _logger.LogError($"Not fount data: {avEx}");
                await HandleExceptionAsync(httpContext, avEx);
            }
            catch(InvalidRequestBodyException avEx)
            {
                _logger.LogError($"Invalid request body: {avEx}");
                await HandleExceptionAsync(httpContext, avEx);
            }
            catch(InvalidcredentialsException avEx)
            {
                _logger.LogError($"Invalid credentials: {avEx}");
                await HandleExceptionAsync(httpContext, avEx);
            }
            catch(NullReferenceException avEx)
            {
                _logger.LogError($"Invalid data maybe value is null: {avEx}");
                await HandleExceptionAsync(httpContext, avEx);
            }
            catch (Exception ex)
            {
                _logger.LogError($"somehting wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = context.Response;

            var errorResponse = new BaseResponseDTO
            {
                IsSuccess = false
            };

            switch (exception)
            {
                case ApplicationException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                case AccessViolationException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                case UnauthorizedAccessException ex:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Message = ex.Message;
                    break;
                case ArgumentOutOfRangeException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                case ArgumentNullException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                case InvalidRequestBodyException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                case EntityNotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                case InvalidcredentialsException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                case NullReferenceException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "Internal server error!";
                    break;
            }

            await context.Response.WriteAsync(new BaseResponseDTO()
            {
                StatusCode = context.Response.StatusCode,
                Message = errorResponse.Message
            }.ToString());
        }

    }
}
