using System;
using Discord;
using Karma.Core.Parsers;

namespace Karma.Core.TypeReaders
{
    public class UserTypeReader : DiscordTypeReader
    {
        public override Type SupportedType => typeof(IUser);

        public override bool TryParse(string value, SystemContext context, out object result)
        {
            if (MentionParser.TryParseUser(value, out ulong id))
            {
                result = context.Client.GetUserAsync(id).GetAwaiter().GetResult();
                return true;
            }

            result = default;
            return false;
        }
    }
}