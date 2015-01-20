
namespace Dk.Interface
{
	public interface IModule
	{
		string GetName();
		bool GetInited();
		bool Initialize();
		void Destroy();
	}
}

