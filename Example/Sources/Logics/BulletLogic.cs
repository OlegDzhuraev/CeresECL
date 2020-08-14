using UnityEngine;

namespace CeresECL.Example
{
	public class BulletLogic : ExtendedBehaviour
	{
		BulletComponent bulletComponent;

		protected override void Init()
		{
			bulletComponent = Get<BulletComponent>();
			
			Entity.Events.Subscribe<ColliderHitEvent>(CheckHit);
		}
		
		protected override void Run()
		{
			bulletComponent.Lifetime -= Time.deltaTime;
			
			if (bulletComponent.Lifetime <= 0)
				Entity.Destroy();
		}

		void CheckHit(ColliderHitEvent hitEvent)
		{
			var otherEntity = hitEvent.HitCollider.GetComponent<Entity>();
		
			if (otherEntity != bulletComponent.Owner)
			{
				Debug.Log("Hit " + otherEntity);

				Entity.Destroy();
				otherEntity.Destroy(); // todo CeresECL add health example
			}
		}
	}
}