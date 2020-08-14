namespace CeresECL
{
    public abstract class Container
    {
        protected Entity Entity { get; }

        protected Container(Entity entity)
        {
            Entity = entity;
        }
        
        public virtual void Init() { }
        public virtual void Run() { }
    }
}