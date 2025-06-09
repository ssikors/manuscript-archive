using Microsoft.AspNetCore.Diagnostics;
using Serilog;

namespace ManuscriptApi.Presentation.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionFeature?.Error;
                    var path = exceptionFeature?.Path;

                    Log.Error(exception, "Unhandled exception at {Path}", path);

                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync("An unexpected error occurred.");
                });
            });

            return app;
        }
    }
}
