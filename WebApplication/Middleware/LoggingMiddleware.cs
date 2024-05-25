using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApplication.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var bodyStr = "";

            // This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();

            // We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            // We convert the byte[] into a string using UTF8 encoding...
            bodyStr = Encoding.UTF8.GetString(buffer);

            //..and finally, assign the read body back to the request body, which is allowed because of EnableBuffering()
            request.Body = new MemoryStream(buffer);

            // Log the required details
            var log = $"[{DateTime.Now}]\nSchema:{request.Scheme} \nHost: {request.Host} \nPath: {request.Path} \nQueryString: {request.QueryString} \nRequest Body: {bodyStr}\n";
            await File.AppendAllTextAsync("logs.txt", log);

            await _next(context);
        }
    }
}