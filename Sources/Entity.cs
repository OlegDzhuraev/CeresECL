using System.Collections.Generic;
using UnityEngine;

namespace CeresECL
{
    /// <summary> Container for all components and logics of your object. Also it is MonoBehaviour, so you can use it as the connection to the Unity API.</summary>
    [DisallowMultipleComponent]
    public sealed class Entity : ExtendedBehaviour
    {
        static readonly List<Entity> entities = new List<Entity>();

        public Tags Tags
        {
            get
            {
                if (!tags)
                    tags = new Tags();

                return tags;
            }
        }
        
        public Events Events 
        {
            get
            {
                if (!events)
                    events = new Events();

                return events;
            }
        }

        Tags tags;
        Events events;

        void Awake() => entities.Add(this);

        public void Destroy()
        {
            entities.Remove(this);
            
            Destroy(gameObject);
        }

        /// <summary> Returns all Entities with specific component. Something like FindObjectsOfType, but faster and works with Ceres ECL.</summary>
        public static List<Entity> FindAllWith<T>() where T : Component
        {
            var resultList = new List<Entity>();
            
            for (var i = 0; i < entities.Count; i++)
                if (entities[i].GetComponent<T>())
                    resultList.Add(entities[i]);
            
            return resultList;
        }

        public static Entity Spawn(GameObject withPrefab)
        {
            var entObject = Instantiate(withPrefab);

            return BuildEntity(entObject);
        }
        
        public static Entity Spawn()
        {
            var entObject = new GameObject("Entity");
            
            return BuildEntity(entObject);
        }

        static Entity BuildEntity(GameObject entObject)
        {
            var entity = entObject.GetComponent<Entity>();
            
            if (!entity)
                entity = entObject.AddComponent<Entity>();

            return entity;
        }
    }
}