using UnityEngine;

namespace CeresECL.Example
{
    public class MoveLogic : ExtendedBehaviour
    {
        MoveComponent moveComponent;

        protected override void Init()
        {
            moveComponent = Entity.Get<MoveComponent>();
        }

        protected override void Run()
        {
            transform.position += moveComponent.Direction * (moveComponent.Speed * Time.deltaTime);
        }
    }
}