using UnityEngine;

namespace CeresECL
{
    /// <summary> Container for all components and logics of your object. Also it is MonoBehaviour, so you can use it as the connection to the Unity API.</summary>
    public sealed class Entity : MonoBehaviour
    {
        public Tags Tags { get; private set; }
        public Events Events { get; private set; }
        public Logics Logics { get; private set; }
        public Components Components { get; private set; }

        void Awake()
        {
            Tags = new Tags(this);
            Events = new Events(this);
            Logics = new Logics(this);
            Components = new Components(this);
        }

        void Update()
        {
            Logics.Run();
            Events.Run();
        }

        public static Entity Spawn<T>(GameObject withPrefab) where T : Builder, new()
        {
            var entObject = Instantiate(withPrefab);
            var entity = entObject.AddComponent<Entity>();
            entity.Logics.Add<T>();
            
            return entity;
        }
        
        public static Entity Spawn<T>() where T : Builder, new()
        {
            var entObject = new GameObject("Entity");
            var entity = entObject.AddComponent<Entity>();
            entity.Logics.Add<T>();
            
            return entity;
        }

        public void Destroy() => Destroy(gameObject);
    }
}