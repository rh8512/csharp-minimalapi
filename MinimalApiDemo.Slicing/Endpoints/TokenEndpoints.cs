using System;
namespace MinimalApiDemo;

public static class TokenEndpoints
{
    public static void UseTokenEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/login", (string login, ITokenService tokenService) =>
        {
            return tokenService.CreateToken(login);
        });
    }
}