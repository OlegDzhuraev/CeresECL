using UnityEngine;

namespace CeresECL
{
	public static class Extensions
	{
		/// <summary> Short version of gameObject.GetComponent Entity (); for MonoBehaviour connections to Ceres ECL. Can return null if object is not Entity.</summary>
		public static Entity GetEntity(this GameObject gameObject) => gameObject.GetComponent<Entity>();

		/// <summary> Returns Component of specified type. Not affects Unity default components because requires T derived from MonoBehaviour - only for game logics. </summary>
		public static T Get<T>(this GameObject gameObject) where T : MonoBehaviour => gameObject.GetComponent<T>();
	}
}