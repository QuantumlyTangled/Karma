using Newtonsoft.Json;

namespace Karma.Core.Configuration
{
    [JsonObject(MemberSerialization.Fields)]
    public class BotLogChannels
    {
        public ulong Common { get; set; }
        public ulong CommandMngr { get; set; }
        public ulong GuildMngr { get; set; }
    }
}