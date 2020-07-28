namespace CeresECL
{
    /// <summary> Builder class is base class to build your entities with your logics. It implements Init Logic. </summary>
    public abstract class Builder : Logic, IInitLogic
    {
        void IInitLogic.Init() => Build();

        protected abstract void Build();
    }
}