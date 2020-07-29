using UnityEngine;

namespace CeresECL.Example
{
    public class MoveLogic : Logic, IInitLogic, IRunLogic
    {
        MoveComponent moveComponent;

        void IInitLogic.Init()
        {
            moveComponent = Entity.Components.Get<MoveComponent>();

            moveComponent.Speed = 2f;
        }

        void IRunLogic.Run()
        {
            Entity.transform.position += moveComponent.Direction * (moveComponent.Speed * Time.deltaTime);
        }
    }
}