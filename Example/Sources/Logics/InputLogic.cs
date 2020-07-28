using UnityEngine;

namespace CeresECL.Example
{
    public class InputLogic : Logic, IInitLogic, IRunLogic
    {
        MoveComponent moveComponent;

        void IInitLogic.Init()
        {
            moveComponent = Entity.Get<MoveComponent>();
        }

        void IRunLogic.Run()
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            
            moveComponent.Direction = new Vector3(x, y, 0);
        }
    }
}