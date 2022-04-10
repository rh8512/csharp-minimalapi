using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApiDemo;

var builder = WebApplication.CreateBuilder(args);

var tokenSettingsSection = builder.Configuration.GetSection("TokenSettings");

builder.Services.Configure<TokenSettings>(tokenSettingsSection);

var tokenSettings = tokenSettingsSection.Get<TokenSettings>();
string issuer = tokenSettings.Issuer;
string audience = tokenSettings.Audience;
string secret = tokenSettings.SecurityKey;

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

builder.Logging.AddFileLogger();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    /*
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestQuery |
                            Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestHeaders |
                            Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProtocol |
                            Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestMethod |
    */
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


app.Logger.LogInformation("Application started");

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseTokenEndpoints();
app.UseAppointmentEndpoints();

app.MapGet("/dashboard", [Authorize] () => "Welcome to the secret Dashboard");

app.Run();

public class TokenSettings
{
    public string SecurityKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}