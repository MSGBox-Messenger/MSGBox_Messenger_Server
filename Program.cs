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
    private static readonly Dictionary<string, string> UserKeys = new();

    public void RegisterKey(string user, string publicKeyBase64)
    {
        UserKeys[user] = publicKeyBase64;
    }

    public string GetUserKey(string user)
    {
        return UserKeys.ContainsKey(user) ? UserKeys[user] : null;
    }

    public async Task SendToGroup(string chatName, string user, string message)
    {
        await Clients.Group(chatName).SendAsync("ReceiveMessage", user, message);
    }

    public async Task JoinChat(string chatName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatName);
    }

    public async Task LeaveChat(string chatName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatName);
    }
}