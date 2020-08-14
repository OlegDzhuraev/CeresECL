using System.Collections.Generic;
using UnityEngine;

namespace CeresECL
{
    /// <summary> Container for all components and logics of your object. Also it is MonoBehaviour, so you can use it as the connection to the Unity API.</summary>
    [DisallowMultipleComponent]
    public sealed class Entity : MonoBehaviour
    {
        static readonly List<Entity> entities = new List<Entity>(CeresSettings.MaxEntities);
        
        public Tags Tags { get; private set; }
        public Components Components { get; private set; }
        public Events Events => events;
        public Logics Logics => logics;
        
        Logics logics;
        Events events;
        
        void Awake()
        {
            Tags = new Tags(this);
            Components = new Components(this);
            
            events = new Events(this);
            logics = new Logics(this);
            
            entities.Add(this);
        }

        void Run() => logics.Run();
        
        public void Destroy()
        {
            entities.Remove(this);
            
            Destroy(gameObject);
        }
        
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

        public static Entity Spawn<T>(GameObject withPrefab) where T : Builder, new()
        {
            var entObject = Instantiate(withPrefab);

            return BuildEntity<T>(entObject);
        }
        
        public static Entity Spawn<T>() where T : Builder, new()
        {
            var entObject = new GameObject("Entity");
            
            return BuildEntity<T>(entObject);
        }

        static Entity BuildEntity<T>(GameObject entObject) where T : Builder, new()
        {
            var entity = entObject.GetComponent<Entity>();
            
            if (!entity)
                entity = entObject.AddComponent<Entity>();
            
            entity.logics.Add<T>();
            
            return entity;
        }
    }
}