using Karma.Core.TypeReaders;

namespace Karma.Core.Factories
{
    public static class DiscordTypeReaderFactoryExtensions
    {
        public static DiscordTypeReaderFactory AddReader(this DiscordTypeReaderFactory factory, DiscordTypeReader reader)
        {
            factory.TryAddReader(reader);
            return factory;
        }
    }
}