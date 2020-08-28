using System.Collections.Generic;
using UnityEngine;

namespace CeresECL
{
    /// <summary> Derive from this class all of your game logic scripts.</summary>
    [RequireComponent(typeof(Entity))]
    public class ExtendedBehaviour : MonoBehaviour
    {
        static readonly List<ExtendedBehaviour> behaviours = new List<ExtendedBehaviour>();
        static readonly List<ExtendedBehaviour> newBehaviours = new List<ExtendedBehaviour>();

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

        [Tooltip("Execution priority of this behaviour Run cycle. Smaller values makes it executing early than others.")]
        [SerializeField, Range(-3, 3)] int executionOrder;

        void Awake()
        {
            newBehaviours.Add(this);

            Init();
        }

        void Start() => PostInit();

        protected virtual void Init() { }
        protected virtual void PostInit() { }
        protected virtual void Run() { }

        /// <summary> Get (with adding if dont exist) any of game components - Logics or Components. </summary>
        public T Get<T>() where T : MonoBehaviour
        {
            var component = GetComponent<T>();

            if (!component)
                component = gameObject.AddComponent<T>();

            return component;
        }
        
        /// <summary> Checks, is there a specified Component on Entity. Argument is MonoBehaviour, so it can be used only for Components and Logics - doesn't affects default Unity Components, only for game logic.</summary>
        public bool Have<T>() where T : MonoBehaviour => GetComponent<T>();

        /// <summary> Removes a specified MonoBehaviour from Entity. It can be component or Logic. Doesn't Affect default Unity Components - Only for Game Logic.</summary>
        public void Delete<T>() where T : MonoBehaviour
        {
            var component = GetComponent<T>();
            
            if (component)
                Destroy(component);
        }
        
        /// <summary> Run game update cycle. It should be done from one place in code. </summary>
        public static void RunAll()
        {
            for (var i = 0; i < newBehaviours.Count; i++)
                newBehaviours[i].AddToRunQueue();
            
            newBehaviours.Clear();

            for (var i = 0; i < behaviours.Count; i++)
                if (behaviours[i].enabled)
                    behaviours[i].Run();
        }
        
        void AddToRunQueue()
        {
            for (var i = 0; i < behaviours.Count; i++)
            {
                if (executionOrder <= behaviours[i].executionOrder)
                {
                    behaviours.Insert(i, this);
                    return;
                }
            }
            
            behaviours.Add(this);
        }

        void OnDestroy() => behaviours.Remove(this);

        /// <summary> Returns Entity with specified Component. Like FindObjectOfType, but faster. </summary>
        public Entity FindWith<T>() where T : Component => Entity.FindWith<T>();
        
        /// <summary> Returns all Entities with specific component. Something like FindObjectsOfType, but faster and works with Ceres ECL.</summary>
        public List<Entity> FindAllWith<T>() where T : Component => Entity.FindAllWith<T>();

        /// <summary> Returns Entity with specified Tag. Like Unity Find method. </summary>
        public Entity FindWith(IntTag tag) => Entity.FindWith(tag);
        
        /// <summary> Returns all Entities with specified Tag. Like Unity Find method. </summary>
        public List<Entity> FindAllWith(IntTag tag) => Entity.FindAllWith(tag);
    }
}