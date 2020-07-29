using UnityEngine;

namespace CeresECL.Example
{
    public class InputLogic : Logic, IInitLogic, IRunLogic
    {
        GameData gameData;
        Camera mainCamera;
        MoveComponent moveComponent;

        void IInitLogic.Init()
        {
            moveComponent = Entity.Components.Get<MoveComponent>();
        }

        void IRunLogic.Run()
        {
            HandleMove();
            
            if (Input.GetMouseButtonDown(0))
                DoShoot();
        }

        void HandleMove()
        {            
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            
            moveComponent.Direction = new Vector3(x, y, 0);
        }

        // todo move to separated shooting logic, keep only input events in this class
        void DoShoot()
        {
            var selfPosition = Entity.transform.position;
            
            var mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = selfPosition.z;
            
            var shootDirection = (mouseWorldPosition - selfPosition).normalized;
           
            var bullet = Entity.Spawn<BulletEntityBuilder>(gameData.BulletPrefab);
            bullet.transform.position = selfPosition;
            
            var bulletMoveComponent = bullet.Components.Get<MoveComponent>();
                
            bulletMoveComponent.Direction = shootDirection;
            bulletMoveComponent.Speed = 6;

            var bulletComponent = bullet.Components.Get<BulletComponent>();
            bulletComponent.Owner = Entity;
            bulletComponent.Damage = 15;
            bulletComponent.Lifetime = 5;
        }
    }
}