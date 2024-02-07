using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace api_uppgift;

public class Program
{
    // Krypteringsfunktion med Rövarspråk
    public static string ToRovarsprak(string text)
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
    public static string FromRovarsprak(string text)
    {
        var result = new System.Text.StringBuilder();
        int i = 0;
        while (i < text.Length)
        {
            if (char.IsLetter(text[i]) && i + 2 < text.Length && text[i + 1] == 'o' && char.ToLower(text[i]) == char.ToLower(text[i + 2]))
            {
                result.Append(text[i]);
                i += 3;
            }
            else
            {
                result.Append(text[i]);
                i++;
            }
        }
        return result.ToString();
    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.UseRouting();

        // Endpoint för att kryptera texten
        app.MapGet("/encrypt", async (HttpContext context) =>
        {
            string textToEncrypt = context.Request.Query["text"];
            string encryptedText = ToRovarsprak(textToEncrypt);

            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(encryptedText);
        });

        // Endpoint för att dekryptera texten
        app.MapGet("/decrypt", async (HttpContext context) =>
        {
            string textToDecrypt = context.Request.Query["text"];
            string decryptedText = FromRovarsprak(textToDecrypt);

            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(decryptedText);
        });

        app.MapGet("/", () =>
        {
            return "Welcome to the Rövarspråk Encrypter!\n"
                + "\n"
                + "How to use:\n"
                + "URL/encrypt?text=[you'r text].\n"
                + "URL/decrypt?text=[you'r text].\n";
        });

        app.Run();
    }
}
