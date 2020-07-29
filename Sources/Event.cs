namespace CeresECL
{
	/// <summary> Derive from this class for your custom Events. Event same to Component, but will live only one frame. </summary>
	public class Event
	{
		public static implicit operator bool(Event evnt)
		{
			return !ReferenceEquals(evnt, null);
		}
	}
}