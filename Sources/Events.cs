using System;
using System.Collections.Generic;

namespace CeresECL
{
    public sealed class Events
    {
        /// <summary> Subscribers of event when Event was added. </summary>
        readonly Dictionary<Type, Action<Event>> subscribers = new Dictionary<Type, Action<Event>>();
        readonly Dictionary<Action<Event>, int> addedDelegatesList = new Dictionary<Action<Event>, int>();

        public void Add<T>(T newEvent) where T : Event
        {
            var type = newEvent.GetType();
            
            if (subscribers.ContainsKey(type))
                subscribers[type].Invoke(newEvent);
        }

        // I really do not like these methods, looks weird 
        public void Subscribe<T>(Action<T> action) where T : Event
        {
            var type = typeof(T);

            Action<Event> eventDelegate = delegate(Event eventInstance) { action?.Invoke((T) eventInstance); };
            
            addedDelegatesList.Add(eventDelegate, action.GetHashCode()); // subs to same method have same hash codes, so this is easy way to check connection between method and action

            if (!subscribers.ContainsKey(type))
                subscribers[type] = eventDelegate;
            else
                subscribers[type] += eventDelegate;
        }
        
        public void Unsubscribe<T>(Action<T> action) where T : Event
        {
            var type = typeof(T);

            foreach (var keyValuePair in addedDelegatesList)
            {
                if (keyValuePair.Value == action.GetHashCode())
                {
                    subscribers[type] -= keyValuePair.Key;
                    addedDelegatesList.Remove(keyValuePair.Key);
                    break;
                }
            }
        }
    }
}