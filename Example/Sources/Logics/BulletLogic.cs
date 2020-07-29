using UnityEngine;

namespace CeresECL.Example
{
	public class BulletLogic : Logic, IInitLogic, IRunLogic
	{
		BulletComponent bulletComponent;
		
		public void Init()
		{
			bulletComponent = Entity.Components.Get<BulletComponent>();
		}
		
		void IRunLogic.Run()
		{
			CheckHit();
			
			bulletComponent.Lifetime -= Time.deltaTime;
			
			if (bulletComponent.Lifetime <= 0)
				Entity.Destroy();
		}

		void CheckHit()
		{
			var hitEvent = Entity.Events.Get<ColliderHitEvent>();

			if (!hitEvent) 
				return;
			
			var otherEntity = hitEvent.HitCollider.GetComponent<Entity>();

			if (otherEntity != bulletComponent.Owner)
			{
				Debug.Log("Hit " + otherEntity);

				Entity.Destroy();
			}
		}
	}
}