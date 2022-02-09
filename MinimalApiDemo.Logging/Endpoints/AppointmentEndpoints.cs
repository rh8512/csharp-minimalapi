using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace MinimalApiDemo;

public static class AppointmentEndpoints
{
    public static void UseAppointmentEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/appointments", (Appointment? appointment) =>
        {
            if (appointment != null)
                return Results.Ok();
            else
                return Results.Problem();
        }).WithTags("Appointments");

        endpointRouteBuilder.MapGet("/html-template", () => Results.Extensions.HtmlResponse(@"<!DOCTYPE html>
            <html lang='en'>
            <meta charset = 'UTF-8'>
            <title> Minimal API Template </title>
            <body>
                <h1>This is HTML response</h1>
                </body>
            </html>
        "));

        endpointRouteBuilder.MapGet("/file", () =>
        {
            return Results.File($"{Environment.CurrentDirectory}/test_image.png", contentType: "image/png");
        });


        endpointRouteBuilder.MapGet("/json", () => new { PatientName = "Piotr", Age=99 });

        endpointRouteBuilder.MapGet("/appointments/{id}", [Authorize] () => "Welcome to the secret appointments by id").WithTags("Appointments"); ;
    }

    record Appointment(string patientName);

}


public class HtmlResult : IResult
{
    private readonly string? code;
    
    public HtmlResult(string htmlCode) => code = htmlCode;

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(code);
        await httpContext.Response.WriteAsync(code);
    }
}

public static class AdditionalResults
{
    public static IResult HtmlResponse(this IResultExtensions extensions, string code) => new HtmlResult(code);
}