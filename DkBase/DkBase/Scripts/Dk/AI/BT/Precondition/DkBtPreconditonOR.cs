using System;

namespace Dk.BehaviourTree
{
	public class DkBtPreconditionOR : DkBtPrecondition
	{
		protected DkBtPrecondition aPrecondition;
		protected DkBtPrecondition bPrecondition;
		
		public DkBtPreconditionOR(DkBtPrecondition A , DkBtPrecondition B)
		{
			aPrecondition = A;
			bPrecondition = B;
		}
		
		override public bool IsTure(DkBtInputParam input)
		{
			return aPrecondition.IsTure(input) || bPrecondition.IsTure(input);
		}
	}
}
