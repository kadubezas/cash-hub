using System.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;

namespace cash.hub.authentication.api.infra.Middleware;

public class TraceMiddleware (RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var request = await Request(context);
        string url = context.Request.GetDisplayUrl();

        using (var activity = Activity.Current?.Source.StartActivity($"Endpoint {url}"))
        {
            activity?.SetTag("ENTRADA", request);

            var originalResponseBody = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await next.Invoke(context);
                var response = await Response(context, responseBody, originalResponseBody);

                activity?.SetTag("SAÍDA", response);
            }
        }
    }

    private static async Task<string> Response(HttpContext context, MemoryStream responseBody, Stream originalResponseBody)
    {
        responseBody.Position = 0;
        var content = await new StreamReader(responseBody).ReadToEndAsync();
        responseBody.Position = 0;
        await responseBody.CopyToAsync(originalResponseBody);
        context.Response.Body = originalResponseBody;
        return content;
    }

    private static async Task<string> Request(HttpContext context)
    {
        context.Request.EnableBuffering();

        string content;
        if (context.Request.Method == "POST" || context.Request.Method == "PUT")
        {
            var requestReader = new StreamReader(context.Request.Body);
            content = await requestReader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }
        else
        {
            content = context.Request.QueryString.ToString();
        }

        return content;
    }
}