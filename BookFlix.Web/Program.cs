using BookFlix.Core;
using BookFlix.Infrastructure;
using BookFlix.Web;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCore();
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

        app.Logger.LogError(
            exceptionHandlerFeature?.Error,
            "Unhandled exception processing {Method} {Path} (TraceId: {TraceId})",
            context.Request.Method,
            context.Request.Path,
            context.TraceIdentifier);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new { error = "ServerErrorMessage" });
    });
});

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "BookFlix API V1"));
app.UseForwardedHeaders();
app.UseCors("BookFlixApiCorsPolicy");
app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
