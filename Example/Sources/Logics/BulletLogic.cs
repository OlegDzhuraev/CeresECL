using UnityEngine;

namespace CeresECL.Example
{
	public class BulletLogic : Logic, IInitLogic, IRunLogic
	{
		BulletComponent bulletComponent;
		
		public void Init()
		{
			bulletComponent = Entity.Components.Get<BulletComponent>();
			
			Entity.Events.Subscribe<ColliderHitEvent>(CheckHit);
		}
		
		void IRunLogic.Run()
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