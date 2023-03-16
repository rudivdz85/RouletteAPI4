using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;

public class ExceptionMiddleware
{
    public static void ConfigureExceptionHandler(IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var error = new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error."
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(error));
                }
            });
        });
    }
}