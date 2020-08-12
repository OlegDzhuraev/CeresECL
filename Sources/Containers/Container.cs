using UnityEngine;

namespace CeresECL
{
    [System.Serializable]
    public abstract class Container
    {
        public Entity Entity => entity;
        
        [SerializeField] Entity entity;
        
        protected Container(Entity entity)
        {
            this.entity = entity;
        }
        
        public virtual void Init() { }
        public virtual void Run() { }
    }
}