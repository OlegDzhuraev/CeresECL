using UnityEngine;

namespace CeresECL
{
	public static class Extensions
	{
		/// <summary> Short version of gameObject.GetComponent Entity (); for MonoBehaviour connections to Ceres ECL. Can return null if object is not Entity.</summary>
		public static Entity GetEntity(this GameObject gameObject) => gameObject.GetComponent<Entity>();
		
		/// <summary> Returns Component of specified type. Not affects Unity default components because requires T derived from Component - only for game logics. </summary>
		public static T Get<T>(this GameObject gameObject) where T : Component => gameObject.GetComponent<T>();
		
		/// <summary> Returns true if game object have Component of specified type. Not affects Unity default components because requires T derived from Component - only for game logics. </summary>
		public static bool Have<T>(this GameObject gameObject) where T : Component => gameObject.Get<T>() != null;
		
		/// <summary> Short version of gameObject.GetComponent Entity (); for MonoBehaviour connections to ECL. Can return null if object is not Entity.</summary>
		public static Entity GetEntity(this MonoBehaviour monoBeh) => monoBeh.GetComponent<Entity>();

		/// <summary> Returns Component of specified type. Not affects Unity default components because requires T derived from Component - only for game logics. </summary>
		public static T Get<T>(this MonoBehaviour monoBeh) where T : Component => monoBeh.GetComponent<T>();
		
		/// <summary> Returns true if game object have Component of specified type. Not affects Unity default components because requires T derived from Component - only for game logics. </summary>
		public static bool Have<T>(this MonoBehaviour monoBeh) where T : Component => monoBeh.Get<T>() != null;
	}
}