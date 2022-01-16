using System;
using Microsoft.AspNetCore.Authorization;

namespace MinimalApiDemo;

public static class AppointmentEndpoints
{
    public static void UseAppointmentEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/appointments", [Authorize] () => "Welcome to the secret appointments");
        endpointRouteBuilder.MapGet("/appointments/{id}", [Authorize] () => "Welcome to the secret appointments by id");
    }
}
