using System.Threading.Tasks;
using Discord.WebSocket;

namespace Karma.Events
{
    public interface IOnMessageSubEvent
    {
        Task ExecuteAsync(SocketMessage message);
    }
}