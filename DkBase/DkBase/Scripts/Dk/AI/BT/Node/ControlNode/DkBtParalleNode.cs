using System;
using System.Collections;
using System.Collections.Generic;

namespace Dk.BehaviourTree
{
	/// <summary>
	/// And
	/// </summary>
	public class DkBtParalleNode : DkBtControlNode
	{
		protected override void Enter (DkBtInputParam input)
		{
			base.Enter (input);
		}

		protected override void Exectue (DkBtInputParam input)
		{
			bool tickChild = false;
			bool busy = false;
			for( int i = 0 ; i < this.m_childNodeList.Count ; i++)
			{
				DkBtNode node = m_childNodeList[i];
				if(node.IsBusy || node.Evaluate(input))
				{
					node.Tick(input);
					if(node.IsFailed)
					{
						node.Finish();
					}
					else if(node.IsBusy)
					{
						tickChild = true;
						busy = true;
					}
					else
					{
						tickChild = true;
					}
				}
			}

			if(!tickChild) // no child run , failed
			{
				this.m_status = eDkBtRuningStatus.Failed;
			}
			else if(busy)
			{
				this.m_status = eDkBtRuningStatus.Running;
			}
			else
			{
				this.m_status = eDkBtRuningStatus.End;
			}

		}

		protected override void Exit (DkBtInputParam input)
		{
			base.Exit (input);
		}				
	}
}
