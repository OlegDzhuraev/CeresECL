namespace CeresECL.Example
{
    public class PlayerEntityBuilder : Builder
    {
		protected override void Build()
        {
           Entity.Logics.Add<InputLogic>();
           Entity.Logics.Add<MoveLogic>();
        }
    }
}