using Dream_House.Models;

namespace Dream_House.Services.Interfaces
{
    public interface IChat
    {
        Task AddMessageAsync(Message message);
        Task DeleteMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesAsync();
    }
}
