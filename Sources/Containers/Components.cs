using System;
using System.Collections.Generic;
using UnityEngine;

namespace CeresECL
{
    [Serializable]
    public sealed class Components : Container
    {
        [SerializeField] List<Component> components;

        public Components(Entity entity) : base(entity)
        {
            components = new List<Component>();
        }
        
        public T Get<T>() where T : Component, new()
        {
            foreach (var component in components)
                if (component is T resultComponent)
                    return resultComponent;

            var newComponent = ScriptableObject.CreateInstance<T>();
            newComponent.Entity = Entity;
            
            components.Add(newComponent);

            return newComponent;
        }
        
        public Component Get(Type type)
        {
            if (!type.IsSubclassOf(typeof(Component)) || Have(type))
                return null;
            
            var newComponent = ScriptableObject.CreateInstance(type) as Component;
            newComponent.Entity = Entity;
            
            components.Add(newComponent);

            return newComponent;
        }

        public bool Have(Type componentType)
        {
            for (var i = 0; i < components.Count; i++)
                if (components[i].GetType() == componentType)
                    return true;

            return false;
        }
        
        public void Delete<T>() where T : Component
        {
            foreach (var component in components)
                if (component is T)
                    components.Remove(component);
        }
        
        public void Delete(Component component)
        {
            for (var i = 0; i < components.Count; i++)
                if (components[i] == component)
                {
                    components.RemoveAt(i);
                    break;
                }
        }

        /// <summary> Used only for editor scripting. </summary>
        public List<Component> GetComponentsEditor() => components;
    }
}