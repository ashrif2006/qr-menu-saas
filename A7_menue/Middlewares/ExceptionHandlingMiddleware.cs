using System.Net;
using System.Text.Json;

namespace A7_menue.Middlewares;


public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _lgger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> lgger)
    {
        _next = next;
        _lgger = lgger;
    }

    public async Task InvokeAsync (HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            _lgger.LogError(ex, "An undled exception occurred.");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                statusCode = 500,
                message = "Something went wrong. Please try again later."
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}

