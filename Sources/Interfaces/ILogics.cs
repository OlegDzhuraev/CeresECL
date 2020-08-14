namespace CeresECL
{
	public interface ILogics
	{
		void Add<T>() where T : Logic, new();
		void Inject(object data);
	}
}