using System.Collections.Generic;
using UnityEngine;

namespace CeresECL
{
    /// <summary> Container for all components and logics of your object. Also it is MonoBehaviour, so you can use it as the connection to the Unity API.</summary>
    public sealed class Entity : MonoBehaviour
    {
        readonly List<Component> components = new List<Component>();
        readonly List<Logic> logics = new List<Logic>();

        void Start()
        {
            foreach (var logic in logics)
                if (logic is IInitLogic initLogic)
                    initLogic.Init();
        }

        void Update()
        {
            foreach (var logic in logics)
                if (logic is IRunLogic runLogic)
                    runLogic.Run();
        }

        public void AddLogic<T>() where T : Logic, new()
        {
            foreach (var logic in logics)
                if (logic is T)
                    return;

            var newLogic = new T
            {
                Entity = this
            };

            if (Application.isPlaying && newLogic is IInitLogic initLogic)
                initLogic.Init(); // todo maybe make first init in update or smth
            
            logics.Add(newLogic);
        }
        
        public T Get<T>() where T : Component, new()
        {
            foreach (var component in components)
                if (component is T resultComponent)
                    return resultComponent;

            var newComponent = new T
            {
                Entity = this
            };
            
            components.Add(newComponent);

            return newComponent;
        }

        public void Delete<T>() where T : Component
        {
            foreach (var component in components)
                if (component is T)
                    components.Remove(component);
        }

        public static Entity Spawn<T>(GameObject withPrefab) where T : Builder, new()
        {
            var entObject = Instantiate(withPrefab);
            var entity = entObject.AddComponent<Entity>();
            entity.AddLogic<T>();
            
            return entity;
        }
        
        public static Entity Spawn<T>() where T : Builder, new()
        {
            var entObject = new GameObject("Entity");
            var entity = entObject.AddComponent<Entity>();
            entity.AddLogic<T>();
            
            return entity;
        }

        /// <summary> Used only for editor scripting. </summary>
        public List<Component> GetComponentsListEditor()
        {
            var list = new List<Component>();

            for (var i = 0; i < components.Count; i++)
                list.Add(components[i]);
            
            return list;
        }
        
        /// <summary> Used only for editor scripting. </summary>
        public List<Logic> GetLogicsListEditor()
        {
            var list = new List<Logic>();

            for (var i = 0; i < logics.Count; i++)
                list.Add(logics[i]);
            
            return list;
        }
    }
}