var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseMiddleware<LoggingMiddleware>();

app.MapGet("/", () => "Running!!!");

app.Run();
