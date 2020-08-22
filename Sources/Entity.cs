using System.Collections.Generic;
using UnityEngine;

namespace CeresECL
{
    /// <summary> Container for all components and logics of your object. Also it is MonoBehaviour, so you can use it as the connection to the Unity API.</summary>
    [DisallowMultipleComponent]
    public sealed class Entity : ExtendedBehaviour
    {
        static readonly List<Entity> entities = new List<Entity>();

        public Tags Tags => tags ?? (tags = new Tags());
        public Events Events => events ?? (events = new Events());

        Tags tags;
        Events events;

        void Awake() => entities.Add(this);

        public void Destroy()
        {
            entities.Remove(this);
            
            Destroy(gameObject);
        }
        
        /// <summary> Returns Entity with specified Tag. Like Unity Find method. </summary>
        public static Entity FindWith(IntTag tag)
        {
            for (var i = 0; i < entities.Count; i++)
                if (entities[i].Tags.Have(tag))
                    return entities[i];

            return null;
        }
        
        /// <summary> Returns all Entities with specified Tag. Like Unity Find method. </summary>
        public static List<Entity> FindAllWith(IntTag tag)
        {
            var resultList = new List<Entity>();
            
            for (var i = 0; i < entities.Count; i++)
                if (entities[i].Tags.Have(tag))
                    resultList.Add(entities[i]);
            
            return resultList;
        }
        
        /// <summary> Returns Entity with specified Component. Like FindObjectOfType, but faster. </summary>
        public static Entity FindWith<T>() where T : Component
        {
            for (var i = 0; i < entities.Count; i++)
                if (entities[i].GetComponent<T>())
                    return entities[i];

            return null;
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
        
        /// <summary> Spawns prefab as Entity. </summary>
        public static Entity Spawn(GameObject withPrefab, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            var entObject = Instantiate(withPrefab);

            return BuildEntity(entObject, position, rotation, parent);
        }
        
        /// <summary> Spawns empty entity with specified name. </summary>
        public static Entity Spawn(string name = "Entity", Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            var entObject = new GameObject(name);
            
            return BuildEntity(entObject, position, rotation, parent);
        }

        static Entity BuildEntity(GameObject entObject, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            var entity = entObject.GetComponent<Entity>();
            
            if (!entity)
                entity = entObject.AddComponent<Entity>();
            
            entObject.transform.position = position;
            entObject.transform.rotation = rotation;
            
            if (parent)
                entObject.transform.SetParent(parent);
            
            return entity;
        }
    }
}