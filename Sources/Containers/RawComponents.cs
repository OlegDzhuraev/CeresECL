using System.Collections.Generic;

namespace CeresECL
{
    public sealed class RawComponents : Container, IComponents
    {
        readonly List<Component> components = new List<Component>();
        
        public RawComponents(Entity entity) : base(entity) { }
        
        public T Get<T>() where T : Component, new()
        {
            foreach (var component in components)
                if (component is T resultComponent)
                    return resultComponent;

            var newComponent = Entity.AddUnityComponent<T>();
            newComponent.Entity = Entity;

            components.Add(newComponent);

            return newComponent;
        }
        
        public bool AddExisting(Component component)
        {
            // todo check entity have this component?
            if (!components.Contains(component))
            {
                component.Entity = Entity;
                components.Add(component);
                return true;
            }

            return false;
        }
        
        public bool Have<T>() where T : Component
        {
            foreach (var component in components)
                if (component is T)
                    return true;

            return false;
        }

        public void Delete<T>() where T : Component
        {
            foreach (var component in components)
                if (component is T)
                    components.Remove(component);
        }
    }
}