using System.Collections.Generic;

namespace Karma.Events
{
    public class EventLoader
    {
        private readonly List<IEvent> _events = new List<IEvent>();

        public EventLoader LoadEvent(IEvent iEvent)
        {
            iEvent.Load();
            _events.Add(iEvent);
            return this;
        }
    }
}