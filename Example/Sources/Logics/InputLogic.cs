using UnityEngine;

namespace CeresECL.Example
{
    public class InputLogic : ExtendedBehaviour
    {
        public GameData GameData;
        public Camera MainCamera;
        
        MoveComponent moveComponent;

        protected override void Init()
        {
            moveComponent = Get<MoveComponent>();
        }

        protected override void Run()
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
            var selfPosition = transform.position;
            
            var mouseWorldPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = selfPosition.z;
            
            var shootDirection = (mouseWorldPosition - selfPosition).normalized;
           
            var bullet = Entity.Spawn(GameData.BulletPrefab);
            bullet.transform.position = selfPosition;

            var bulletMoveComponent = bullet.Get<MoveComponent>();
            bulletMoveComponent.Direction = shootDirection;

            var bulletComponent = bullet.Get<BulletComponent>();
            bulletComponent.Owner = Entity;
        }
    }
}