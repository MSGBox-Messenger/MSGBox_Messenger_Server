using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<ChatHub>("/chat");

app.MapGet("/", () => "The MSGBox Messager Server is online");

app.Run();

// Message Server main class
// Take messages an send it to other clients
// Create private rooms
public class ChatHub : Hub
{

    public async Task SendToGroup(string chatName, string user, string message)
    {
        await Clients.Group(chatName).SendAsync("ReceiveMessage", user, message);
    }

    public async Task JoinChat(string chatName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatName);
    }
}