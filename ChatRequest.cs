namespace HorizonGPT.API;

public class ChatRequest
{
    // public Microsoft.Extensions.AI.ChatMessage Message { get; set; }
    public string Message { get; set; }
    public string Role { get; set; } // "user" or "assistant"
}

