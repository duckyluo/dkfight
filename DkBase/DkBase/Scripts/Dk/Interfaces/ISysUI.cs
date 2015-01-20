
namespace Dk.Interface
{
	public interface ISysUI : IComponent
	{
		void SetMediator(IDkMediator mediator);
		bool IsShowByScene(string scene);
	}


}

