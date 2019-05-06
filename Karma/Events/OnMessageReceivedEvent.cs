using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Karma.Events
{
    public class OnMessageReceivedEvent : IEvent
    {
        private readonly DiscordShardedClient _client;
        private readonly List<IOnMessageSubEvent> _subEvents = new List<IOnMessageSubEvent>();

        public OnMessageReceivedEvent(DiscordShardedClient client)
        {
            _client = client;
        }

        public void Load() {
            _client.MessageReceived += message => Task.Factory.StartNew(() => ExecuteReceivedAsync(message));
            _client.MessageUpdated += (oldMessage, newMessage, channel) => Task.Factory.StartNew(() => ExecuteUpdatedAsync(newMessage));
        }

        public OnMessageReceivedEvent AddSubEvent(IOnMessageSubEvent sEvent)
        {
            _subEvents.Add(sEvent);
            return this;
        }

        private Task ExecuteReceivedAsync(SocketMessage message)
        {
            ExecuteAsync(message).GetAwaiter();
            return Task.CompletedTask;
        }

        private Task ExecuteUpdatedAsync(SocketMessage message)
        {
            ExecuteAsync(message).GetAwaiter();
            return Task.CompletedTask;          
        }

        private Task ExecuteAsync(SocketMessage message)
        {
            if (!(message is SocketUserMessage) || !(message.Channel is SocketGuildChannel) || string.IsNullOrEmpty(message.Content)) return Task.CompletedTask;

            foreach (var subEvent in _subEvents)
                subEvent.ExecuteAsync(message).GetAwaiter();
            return Task.CompletedTask;
        }
    }
}