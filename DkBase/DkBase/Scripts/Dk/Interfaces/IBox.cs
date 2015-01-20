
namespace Dk.Interface
{
	public enum eBoxType
	{
		NORMAL = 0,
		SYSTEM = 1,
	}

	public interface IBox : IComponent
	{
		void SetMediator(IDkMediator mediator);
		eBoxType GetType();
	}


}

