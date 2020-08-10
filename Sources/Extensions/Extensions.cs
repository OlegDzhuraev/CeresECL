using UnityEngine;

namespace CeresECL
{
	public static class Extensions
	{
		/// <summary> Short version of gameObject.GetComponent Entity (); for MonoBehaviour connections to Ceres ECL. Can return null if object is not Entity.</summary>
		public static Entity GetEntity(this GameObject gameObject) => gameObject.GetComponent<Entity>();
	}
}