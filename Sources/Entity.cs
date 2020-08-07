using System.Collections.Generic;
using UnityEngine;

namespace CeresECL
{
    /// <summary> Container for all components and logics of your object. Also it is MonoBehaviour, so you can use it as the connection to the Unity API.</summary>
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
        }

        void Run() => logics.Run();
        
        public static void UpdateAll()
        {
            for (var i = 0; i < entitiesCount; i++)
                entities[i].Run();
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
            var entity = entObject.AddComponent<Entity>();
            entity.logics.Add<T>();
            
            entities.Add(entity);
            
            return entity;
        }

        public void Destroy()
        {
            entities.Remove(this);
            
            Destroy(gameObject);
        }
    }
}