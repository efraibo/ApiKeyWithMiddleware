

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace TestMiddleware;

public class ApiKeyMiddleware {
    private readonly RequestDelegate _next;
    private const string APIKEY = "XApiKey";
    private readonly IConfiguration _configuration;
    
    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration) {
        _next = next;
        _configuration = configuration;
    }
    
    public async Task InvokeAsync(HttpContext context) {
        if (!context.Request.Headers.TryGetValue(APIKEY, out
                var extractedApiKey)) {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Api Key was not provided ");
            return;
        }
        var apiKey = _configuration[APIKEY];

        if (!apiKey.Equals(extractedApiKey)) {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Unauthorized client");
            return;
        }
        await _next(context);
    }
}
