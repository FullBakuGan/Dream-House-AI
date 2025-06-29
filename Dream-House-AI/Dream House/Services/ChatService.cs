using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dream_House.Models;
using Dream_House.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
public class ChatService : IChat
{
    private readonly List<Message> _messages = new List<Message>();

    public Task<IEnumerable<Message>> GetMessagesAsync()
    {
        return Task.FromResult<IEnumerable<Message>>(_messages);
    }

    public Task AddMessageAsync(Message message)
    {
        _messages.Add(message);
        return Task.CompletedTask;
    }

    public Task DeleteMessageAsync(Message message)
    {
        _messages.Remove(message);
        return Task.CompletedTask;
    }
}