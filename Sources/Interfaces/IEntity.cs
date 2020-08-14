using System.Collections.Generic;
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

		IEntity Spawn<T>() where T : Entity, new();
		IEntity Spawn<T>(GameObject prefab) where T : Entity, new();
		
		T AddUnityComponent<T>() where T : Behaviour;
		T GetUnityComponent<T>() where T : Behaviour;
		List<IEntity> FindAllWith<T>() where T : Component;
		
		void Destroy();
	}
}