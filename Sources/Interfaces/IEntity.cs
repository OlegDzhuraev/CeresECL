using UnityEngine;

namespace CeresECL
{
	public interface IEntity
	{
		ILogics Logics { get; }
		IComponents Components { get; }
		Events Events { get; }
		Tags Tags { get; }
		
		Transform Transform { get; }
		GameObject GameObject { get; }

		T Spawn<T>() where T : Entity, new();
		T Spawn<T>(GameObject prefab) where T : Entity, new();
		
		T AddUnityComponent<T>() where T : MonoBehaviour;
		T GetUnityComponent<T>() where T : MonoBehaviour;

		void Destroy();
	}
}