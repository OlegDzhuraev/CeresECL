using System.Collections.Generic;
using UnityEngine;

namespace CeresECL
{
    /// <summary> Derive from this class all of your game logic scripts.</summary>
    [RequireComponent(typeof(Entity))]
    public class ExtendedBehaviour : MonoBehaviour
    {
        static readonly List<ExtendedBehaviour> behaviours = new List<ExtendedBehaviour>();

        public Entity Entity
        {
            get
            {
                if (!entity)
                    entity = GetComponent<Entity>();

                return entity;
            }
        }
        
        Entity entity;

        void Awake()
        {
            behaviours.Add(this);
            Init();
        }

        protected virtual void Init() { }
        protected virtual void Run() { }

        /// <summary> Get (with adding if dont exist) any of game components - Logics or Components. </summary>
        public T Get<T>() where T : MonoBehaviour
        {
            var component = GetComponent<T>();

            if (!component)
                component = gameObject.AddComponent<T>();

            return component;
        }

        public bool Have<T>() where T : MonoBehaviour => GetComponent<T>();

        public void Delete<T>() where T : MonoBehaviour
        {
            var component = GetComponent<T>();
            
            if (component)
                Destroy(component);
        }
        
        /// <summary> Run game update cycle. It should be done from one place in code. </summary>
        public static void RunAll()
        {
            for (var i = 0; i < behaviours.Count; i++)
                behaviours[i].Run();
        }

        void OnDestroy() => behaviours.Remove(this);
    }
}