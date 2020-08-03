using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CeresECL
{
    public sealed class Events : Container
    {
        /// <summary> Subscribers of event when Event was added. </summary>
        readonly Dictionary<Type, Action<Event>> subscribers = new Dictionary<Type, Action<Event>>();
        readonly Dictionary<Action<Event>, int> addedDelegatesList = new Dictionary<Action<Event>, int>();
        
        readonly List<Event> events = new List<Event>();
        
        public Events(Entity entity) : base(entity) { }
        
        public override void Run()
        {
            foreach (var receivedEvent in events)
            {
                var type = receivedEvent.GetType();
            
                if (subscribers.ContainsKey(type))
                    subscribers[type].Invoke(receivedEvent);
            }

            events.Clear();
        }

        public T Add<T>() where T : Event, new()
        {
            var newEvent = new T();

            events.Add(newEvent);
            
            return newEvent;
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