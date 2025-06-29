using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    public async Task SendMessageToUser(string targetUserId, string message)
    {
        var senderName = Context.User.Identity.Name;
        Console.WriteLine($"Отправка: {senderName} → {targetUserId}: {message}");

        await Clients.User(targetUserId).SendAsync("ReceiveMessage", new
        {
            sender = senderName,
            text = message
        });

        await Clients.Caller.SendAsync("ReceiveMessage", new
        {
            sender = "Вы",
            text = message
        });
    }

    public override async Task OnConnectedAsync()
    {
        if (string.IsNullOrEmpty(Context.UserIdentifier))
        {
            Console.WriteLine("Ошибка: UserIdentifier не установлен!");
            await Clients.Caller.SendAsync("ReceiveError", "Требуется авторизация");
            Context.Abort();
            return;
        }

        Console.WriteLine($"Новое подключение: {Context.UserIdentifier}");
        await base.OnConnectedAsync();
    }
}