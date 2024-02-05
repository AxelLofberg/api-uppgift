using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.UseRouting();

        // Krypteringsfunktion med Rövarspråk
        string ToRovarsprak(string text)
        {
            var result = new System.Text.StringBuilder();

            foreach (char c in text)
            {
                if (char.IsLetter(c) && !"aeiouyåäöAEIOUYÅÄÖ".Contains(c))
                {
                    result.Append(c);
                    result.Append('o');
                    result.Append(char.ToLower(c));
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        // Dekrypteringsfunktion
        string FromRovarsprak(string text)
        {
            var result = new System.Text.StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                result.Append(text[i]);
                if (i < text.Length - 2 && text[i] == 'o' && char.ToLower(text[i + 1]) == text[i + 2])
                {
                    i += 2;
                }
            }
            return result.ToString();
        }

        app.MapGet("/", async (HttpContext context) =>
        {
            var htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "index.html");
            var htmlContent = await File.ReadAllTextAsync(htmlFilePath);

            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(htmlContent);
        });

        app.MapPost("/encrypt", async (HttpContext context) =>
        {
            using var reader = new StreamReader(context.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var encryptedText = ToRovarsprak(requestBody);

            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(encryptedText);
        });

        app.MapPost("/decrypt", async (HttpContext context) =>
        {
            using var reader = new StreamReader(context.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var decryptedText = FromRovarsprak(requestBody);

            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(decryptedText);
        });

        app.MapGet("/hello", async (HttpContext context) =>
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Hello, World!");
        });

        app.MapGet("/goodbye", async (HttpContext context) =>
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Goodbye, World!");
        });

        app.Run();
    }
}
