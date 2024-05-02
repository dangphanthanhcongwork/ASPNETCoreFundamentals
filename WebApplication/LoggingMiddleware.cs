using System.Text;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _filePath = "logs.txt";

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var request = context.Request;

            var log = new StringBuilder();
            log.AppendLine($"{DateTime.Now}");
            log.AppendLine($"Schema: {request.Scheme}");
            log.AppendLine($"Host: {request.Host}");
            log.AppendLine($"Path: {request.Path}");
            log.AppendLine($"QueryString: {request.QueryString}");

            request.EnableBuffering();
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            log.AppendLine($"Request Body: {body}");
            // reset the request body stream position so the next middleware can read it
            request.Body.Position = 0;

            log.AppendLine("------------------------------------------------------------------");

            // write the log to the file
            await File.AppendAllTextAsync(_filePath, log.ToString());

            // write the log to the console
            Console.Write(log.ToString());

            await _next(context);
        }
        catch (Exception ex)
        {
            var log = new StringBuilder();
            log.AppendLine($"Exception: {ex.Message}");
            await File.AppendAllTextAsync(_filePath, log.ToString());
            Console.Write(log.ToString());
            throw; // rethrow the exception after logging it
        }
    }
}