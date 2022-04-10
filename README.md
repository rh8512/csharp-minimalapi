# csharp-minimalapi
Minimal Api Demo:

**1.MinimalApiDemo.BasicJWT** - simple example, no databases, even without checking the login and password, it's supposed to be a minimum, just pure token creation

**2.MinimalApiDemo.DIExample** - simple as above, but I introduced here TokenService and used IConfiguration & IOptions to access appsettings.json settings

**3.MinimalApiDemo.SlicingEndpoints** - simple as above, but I extracted endpoints to external files using extension methods

**4.MinimalApiDemo.EndpointsResponses** - custom result type (HTML result) using IResult interface

**5.MinimalApiDemo.Logging** - custom file logger, build using ILogger & ILoggerProvider
**6.MinimalApiDemo.AWSLambdaHosting** - how to host Minimal API using AWS Lambda Service
