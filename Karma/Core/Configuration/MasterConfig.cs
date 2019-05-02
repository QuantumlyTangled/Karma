using System;
using System.IO;
using Newtonsoft.Json;
using Karma.Core.Enums;

namespace Karma.Core.Configuration
{
    [JsonObject(MemberSerialization.Fields)]
    public class MasterConfig
    {
        [JsonIgnore]
        private const string Filename = "config.json";
        
        public DiscordConfig DiscordConfig { get; }
        
        [JsonConstructor]
        public MasterConfig(DiscordConfig discordConfig)
        {
            DiscordConfig = discordConfig;
        }
        
        private static string SetupInput(string query)
        {
            Console.Write(query);
            return Console.ReadLine();
        }

        private static string SetupInput(string query, string defaultTo)
        {
            Console.Write(query);
            var output = Console.ReadLine();
            return string.IsNullOrWhiteSpace(output) ? defaultTo: output;
        }

        public static MasterConfig Load() 
        {
            if (!File.Exists(Filename)) return Setup();
            return JsonConvert.DeserializeObject<MasterConfig>(File.ReadAllText(Filename));
        }
        
        private static MasterConfig Setup()
        {
            var token = SetupInput("Discord || Token: ");
            var prefix = SetupInput("Discord || Prefix [!]: ", "!");
            var masterAdminId = ulong.Parse(SetupInput("Discord || Master Admin ID: "));
            var discordConfig = new DiscordConfig(token, prefix, masterAdminId);

            var masterConfig = new MasterConfig(discordConfig);
            masterConfig.Save();

            return masterConfig;
        }
        
        private void Save()
            => File.WriteAllText(Filename, JsonConvert.SerializeObject(this, Formatting.Indented));
        
        public void AssignMasterGuild(ulong guildId)
        {
            DiscordConfig.AssignMasterGuild(guildId);
            Save();
        }

        public void AssignBotLogChannel(ulong channelId, BotLogType logType)
        {
            DiscordConfig.AssignBotLogChannel(channelId, logType);
            Save();
        }

        public void AssignPrefix(string prefix)
        {
            DiscordConfig.Prefix = prefix;
            Save();
        }

        public bool AssignAdmin(ulong userId)
        {
            if (!DiscordConfig.AssignAdmin(userId)) return false;
            Save();
            return true;
        }

        public bool RemoveAdmin(ulong userId)
        {
            if (!DiscordConfig.RemoveAdmin(userId)) return false;
            Save();
            return true;
        }
    }
}