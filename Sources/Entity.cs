using System.Collections.Generic;
using UnityEngine;

namespace CeresECL
{
    /// <summary> Container for all components and logics of your object. Also it is MonoBehaviour, so you can use it as the connection to the Unity API.</summary>
    [DisallowMultipleComponent]
    public abstract class Entity : MonoBehaviour, IEntity
    {
        static readonly List<Entity> entities = new List<Entity>(CeresSettings.MaxEntities);

        public Tags Tags => tags;
        public Events Events => events;

        public IComponents Components => components;
        public ILogics Logics => logics;
        
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
        
        Tags tags;
        Events events;
        
        RawComponents components;
        RawLogics logics;
        
        void Awake()
        {
            tags = new Tags(this);
            events = new Events(this);
            components = new RawComponents(this);
            logics = new RawLogics(this);

            entities.Add(this);
            
            var gatheredLogics = GetComponents<Logic>();
            
            for (var i = 0; i < gatheredLogics.Length; i++)
                if (!logics.AddExisting(gatheredLogics[i]))
                    Destroy(gatheredLogics[i]);
            
            var gatheredComponents = GetComponents<Component>();

            for (var i = 0; i < gatheredComponents.Length; i++)
                if (!components.AddExisting(gatheredComponents[i]))
                    Destroy(gatheredComponents[i]);
            
            Build();
        }

        protected virtual void Build() { }

        void Run() => logics.Run();
        
        public void Destroy()
        {
            entities.Remove(this);
            
            Destroy(gameObject);
        }

        public T AddUnityComponent<T>() where T : MonoBehaviour => gameObject.AddComponent<T>();
        public T GetUnityComponent<T>() where T : MonoBehaviour => gameObject.GetComponent<T>();
        
        T IEntity.Spawn<T>() => Spawn<T>();
        T IEntity.Spawn<T>(GameObject prefab) => Spawn<T>(prefab);
        
        /// <summary> Run game update cycle. It should be done from one place in code. </summary>
        public static void UpdateAll()
        {
            for (var i = 0; i < entities.Count; i++)
                entities[i].Run();
        }
        
        /// <summary> Returns all Entities with specific component. Something like FindObjectsOfType, but faster and works with Ceres ECL.</summary>
        public static List<Entity> FindAllWith<T>() where T : Component
        {
            var resultList = new List<Entity>();
            
            for (var i = 0; i < entities.Count; i++)
                if (entities[i].Components.Have<T>())
                    resultList.Add(entities[i]);
            
            return resultList;
        }

        public static T Spawn<T>(GameObject withPrefab) where T : Entity, new()
        {
            var entObject = Instantiate(withPrefab);

            return BuildEntity<T>(entObject);
        }
        
        public static T Spawn<T>() where T : Entity, new()
        {
            var entObject = new GameObject("Entity");
            
            return BuildEntity<T>(entObject);
        }

        static T BuildEntity<T>(GameObject entObject) where T : Entity, new()
        {
            var entity = entObject.GetComponent<T>();
            
            if (!entity)
                entity = entObject.AddComponent<T>();

            return entity;
        }
    }
}