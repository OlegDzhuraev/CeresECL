namespace CeresECL
{
	public interface IComponents
	{
		T Get<T>() where T : Component, new();
		bool Have<T>() where T : Component;
		void Delete<T>() where T : Component;
	}
}