using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApiDemo;

var builder = WebApplication.CreateBuilder(args);

string issuer = builder.Configuration.GetValue<string>("Issuer");
string audience = builder.Configuration.GetValue<string>("Audience");
string secret = builder.Configuration.GetValue<string>("SecurityKey");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JSON Web Token",
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement() {
        {new OpenApiSecurityScheme {Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme,Id = "Bearer"}},Array.Empty<string>()}}
    );
});
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
    };
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/login", (string login, ITokenService tokenService) =>
{
    return tokenService.CreateToken(login);
})
.WithName("GetToken");


app.MapGet("/dashboard", [Authorize] () => "Welcome to the secret Dashboard");

app.Run();