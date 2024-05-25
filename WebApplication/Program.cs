using WebApplication.Middleware;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseMiddleware<LoggingMiddleware>();

app.MapGet("/", () => "Hello World!");

app.Run();
