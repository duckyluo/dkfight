using System;

namespace Dk.BehaviourTree
{
	public class DkBtPreconditionAND : DkBtPrecondition
	{
		protected DkBtPrecondition aPrecondition;
		protected DkBtPrecondition bPrecondition;

		public DkBtPreconditionAND(DkBtPrecondition A , DkBtPrecondition B)
		{
			aPrecondition = A;
			bPrecondition = B;
		}

		override public bool IsTure(DkBtInputParam input)
		{
			return aPrecondition.IsTure(input) && bPrecondition.IsTure(input);
		}
	}
}
