# hozion-gpt-demo
This project demonstrates how to build a chat backend using ASP.NET Core, leveraging the Microsoft.Extensions.AI library to integrate multiple AI services and models seamlessly.

## Overview
The primary goal of this backend is to showcase the capabilities of the Microsoft.Extensions.AI library, which provides unified abstractions for interacting with various AI services. By utilizing this library, developers can easily switch between different AI models or services without significant code changes, promoting flexibility and scalability in AI-driven applications.

## Features
- Unified AI Integration: Utilizes Microsoft.Extensions.AI to interact with multiple AI services through a consistent interface.
- Streaming Responses: Implements streaming of AI-generated responses to clients for improved user experience.
- CORS Support: Configures Cross-Origin Resource Sharing (CORS) to allow requests from any origin, facilitating integration with various front-end applications.
- Customizable Models: Demonstrates how to switch between different AI models or services by updating configuration settings, enabling easy experimentation and optimization.

## Prerequisites
- .NET 9.0 SDK or later [Download .NET SDK](https://dotnet.microsoft.com/download)
- An AI service endpoint (e.g., [Ollama](https://ollama.com/)) running locally or accessible over the network.

## Getting Started
1. Clone the repository:
   ```bash
   git clone https://github.com/sohan88/hozion-gpt-demo.git
   cd hozion-gpt-demo
    ```
   
2. Install the required dependencies:
   ```bash
   dotnet restore
   ```
   
3. Configure AI Client in Prgram.cs
    ```csharp
   using Microsoft.Extensions.AI;
   using Microsoft.Extensions.DependencyInjection;
   var builder = WebApplication.CreateBuilder(args);// Add AI chat client
   builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:11434"), "llama3:8b-instruct-q2_K"));
   var app = builder.Build();
    ```
   
4. Run the application:
    ```bash
    dotnet run
    ```

This project serves as a foundational example of integrating multiple AI services into an ASP.NET Core backend using the Microsoft.Extensions.AI library. Developers can expand upon this template to build more complex and feature-rich AI-driven applications.
   