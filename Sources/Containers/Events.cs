using System;
using System.Collections.Generic;

namespace CeresECL
{
    public sealed class Events : Container
    {
        readonly Dictionary<Type, Action<Event>> subscribers = new Dictionary<Type, Action<Event>>();
        readonly Dictionary<Action<Event>, int> addedDelegates = new Dictionary<Action<Event>, int>();

        /// <summary> Method allows send Event to the entity. </summary>
        public void Add<T>(T newEvent) where T : Event
        {
            var type = newEvent.GetType();

            if (subscribers.ContainsKey(type))
                subscribers[type].Invoke(newEvent);
        }

        /// <summary> Simplier version of Add. If you do not need to pass any data to this event. </summary>
        public void Add<T>() where T : Event, new() => Add(new T());

        public void Sub<T>(Action<T> action) where T : Event =>
            Sub(typeof(T), e => action?.Invoke((T) e), action.GetHashCode());

        public void Sub<T>(Action action) where T : Event =>
            Sub(typeof(T), e => action?.Invoke(), action.GetHashCode());

        void Sub(Type type, Action<Event> eventDelegate, int rawActionHash)
        {
            if (!subscribers.ContainsKey(type))
                subscribers.Add(type, eventDelegate);
            else
                subscribers[type] += eventDelegate;

            addedDelegates.Add(eventDelegate,
                rawActionHash); // subs to same method have same hash codes, so this is easy way to check connection between method and action
        }

        public void Unsub<T>(Action<T> action) where T : Event => Unsub<T>(action.GetHashCode());
        public void Unsub<T>(Action action) where T : Event => Unsub<T>(action.GetHashCode());

        void Unsub<T>(int actionHash) where T : Event
        {
            var type = typeof(T);

            foreach (var keyValuePair in addedDelegates)
                if (keyValuePair.Value == actionHash)
                {
                    subscribers[type] -= keyValuePair.Key;

                    if (subscribers[type] == null)
                        subscribers.Remove(type);

                    addedDelegates.Remove(keyValuePair.Key);

                    break;
                }
        }

        public Events(Entity entity) : base(entity) { }
    }
}