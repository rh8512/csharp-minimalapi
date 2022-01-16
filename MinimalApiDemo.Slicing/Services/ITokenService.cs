using System;
namespace MinimalApiDemo;
public interface ITokenService
{
    public string CreateToken(string login);
}