using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

const string _issuer = "pp";
const string _audience = "pp";
const string _secret = "supersecretkey :)";

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

    s.AddSecurityRequirement(new OpenApiSecurityRequirement() {{new OpenApiSecurityScheme {Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme,Id = "Bearer"}},Array.Empty<string>()}});
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = _issuer,
        ValidAudience = _audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret))
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


app.MapGet("/login", (string login, string password, HttpContext http) =>
{
    //checking login & password - for demo open for all users :)
    var claims = new[] {
        new Claim(ClaimTypes.Actor, login),
        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
    };

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
    var tokenDescriptor = new JwtSecurityToken(_issuer, _audience, claims,
        expires: DateTime.Now.Add(new TimeSpan(0, 5, 0)), signingCredentials: credentials);
    return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
})
.WithName("GetToken");


app.MapGet("/dashboard", [Authorize] () => "Welcome to the secret Dashboard");

app.Run();