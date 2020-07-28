using CeresECL.Example;

namespace CeresECL
{
    public class PlayerEntityBuilder : Builder
    {
        protected override void Build()
        {
           Entity.AddLogic<InputLogic>();
           Entity.AddLogic<MoveLogic>();
        }
    }
}