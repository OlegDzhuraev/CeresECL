using UnityEngine;

namespace CeresECL.Example
{
	public class BulletComponent : MonoBehaviour
	{
		[HideInInspector] public Entity Owner;
		
		public float Damage;
		public float Lifetime;
	}
}