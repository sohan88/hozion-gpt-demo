using System.Text;
using System.Text.Json;
using HorizonGPT.API;
using Microsoft.Extensions.AI;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

var builder = WebApplication.CreateBuilder(args);
var chatHistory = new List<ChatMessage>();

// Add Ollama chat client
builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:11434"), "llama3:8b-instruct-q2_K"));

builder.Services.AddCors(op =>
{
    op.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin(); // Allow requests from any origin
    });
});

var app = builder.Build();
app.UseCors("AllowAll"); // Apply the CORS policy

app.MapPost("/chat", async (HttpContext context, IChatClient chatClient) =>
{
    // Read the request body as a ChatRequest
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    Console.WriteLine($"Received request body: {requestBody}"); // Log the incoming request

    var request = JsonSerializer.Deserialize<ChatRequest>(requestBody);

    // Check if the request is valid
    if (string.IsNullOrEmpty(request?.Message))
    {
        context.Response.StatusCode = 400; // Bad Request
        await context.Response.WriteAsync("Invalid chat request.");
        return;
    }

    // Create the ChatMessage with the appropriate role and content
    var role = request.Role?.ToLower() == "assistant" ? ChatRole.Assistant : ChatRole.User;
    var chatMessage = new ChatMessage(role, request.Message);
    chatHistory.Add(chatMessage);

    // Set response content type to JSON
    context.Response.ContentType = "application/json";

    // Use a buffer to collect tokens before sending
    var buffer = new StringBuilder();
    var bufferSize = 5; // Number of tokens to buffer before sending
    var tokenCount = 0;

    await foreach (var item in chatClient.CompleteStreamingAsync(chatHistory))
    {
        buffer.Append(item.Text);
        tokenCount++;

        // Send the buffer if it reaches the desired size
        if (tokenCount >= bufferSize)
        {
            await context.Response.WriteAsync(buffer.ToString());
            await context.Response.Body.FlushAsync();
            buffer.Clear();
            tokenCount = 0;
        }
    }

    // Send any remaining tokens in the buffer
    if (buffer.Length > 0)
    {
        await context.Response.WriteAsync(buffer.ToString());
        await context.Response.Body.FlushAsync();
    }

    // Add assistant's response to chat history for context in future requests
    chatHistory.Add(new ChatMessage(ChatRole.Assistant, buffer.ToString()));
});

app.Run();