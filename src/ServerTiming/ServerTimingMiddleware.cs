using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ServerTiming;

public class ServerTimingMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Response.OnStarting(WriteServerTimingHeader, context);
        return next(context);
    }

    private static Task WriteServerTimingHeader(object ctx)
    {
        var context = (HttpContext)ctx;

        var tracker = context.RequestServices.GetRequiredService<ServerTimingTracker>();
        context.Response.Headers.Append("Server-Timing", tracker.GetHeaderValues());

        return Task.CompletedTask;
    }
}
