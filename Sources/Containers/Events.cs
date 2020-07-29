using System.Collections.Generic;

namespace CeresECL
{
    public sealed class Events : Container
    {
        readonly List<Event> events = new List<Event>();
        
        public Events(Entity entity) : base(entity) { }
        
        public override void Run() => events.Clear();
        
        public bool Have<T>() where T : Event => Get<T>();
        
        public T Get<T>() where T : Event
        {
            foreach (var evnt in events)
                if (evnt is T typedEvent)
                    return typedEvent;
            
            return null;
        }
        
        public T Add<T>() where T : Event, new()
        {
            var newEvent = new T();

            events.Add(newEvent);

            return newEvent;
        }
        
        // todo make subscriptions like in tags?
    }
}