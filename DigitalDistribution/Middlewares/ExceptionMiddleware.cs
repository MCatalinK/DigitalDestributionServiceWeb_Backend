using DigitalDistribution.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;
using System;
using Serilog;
using System.Net;
using System.Threading.Tasks;
using ILogger = NLog.ILogger;

namespace DigitalDistribution.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value!.Contains("/api/"))
            {
                try
                {
                    await _next(context);
                }
                catch (BadRequestException ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var response = JsonConvert.SerializeObject(new ExceptionResponse
                    {
                        Message = ex.Message,
                        Trace = ex.StackTrace,
                        Type = ex.GetType().Name
                    });

                    await context.Response.WriteAsync(response);
                    _logger.Info(response);
                    Log.Error(ex, "Bad request exception");
                }
                catch (NotFoundException ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    var response = JsonConvert.SerializeObject(new ExceptionResponse
                    {
                        Message = ex.Message,
                        Trace = ex.StackTrace,
                        Type = ex.GetType().Name
                    });

                    await context.Response.WriteAsync(response);
                    _logger.Info(response);
                    Log.Error(ex, "Not found exception");
                }
                catch (ItemExistsException ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    var response = JsonConvert.SerializeObject(new ExceptionResponse
                    {
                        Message = ex.Message,
                        Trace = ex.StackTrace,
                        Type = ex.GetType().Name
                    });

                    await context.Response.WriteAsync(response);
                    _logger.Info(response);
                    Log.Error(ex, "Item already exists exception");
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new ExceptionResponse
                    {
                        Message = ex.Message,
                        Trace = ex.StackTrace,
                        Type = ex.GetType().Name
                    }));
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
