using System;
using System.Collections;
using System.Collections.Generic;

namespace Dk.BehaviourTree
{
	/// <summary>
	/// sequence one by one
	/// </summary>
	public class DkBtSequenceNode : DkBtControlNode
	{	
		protected int curNodeIndex = 0;

		protected override void Enter (DkBtInputParam input)
		{
			curNodeIndex = 0;
			base.Enter (input);
		}
		
		protected override void Exectue (DkBtInputParam input)
		{
			bool tickChild = false;
			while(true)
			{
				int curIndex = curNodeIndex;
				if(m_childNodeList.Count > curIndex) //have next
				{
					DkBtNode curNode = m_childNodeList[curIndex];
					if(!curNode.IsBusy && !curNode.Evaluate(input))
					{
						curNodeIndex++;
						continue;
					}
					
					curNode.Tick(input);
				
					if(curNode.IsFailed)
					{
						curNode.Finish();
						curNodeIndex++;
						continue;
					}
					else if(curNode.IsBusy)
					{
						tickChild = true;
						this.m_status = eDkBtRuningStatus.Running;
						break;
					}
					else
					{
						tickChild = true;
						curNodeIndex++;
						continue;
					}
				}
				else
				{
					curNodeIndex = 0;
					if(tickChild)
					{
						this.m_status = eDkBtRuningStatus.End;
					}
					else
					{
						this.m_status = eDkBtRuningStatus.Failed;
					}

					break;
				}
			}

		}
		
		protected override void Exit (DkBtInputParam input)
		{
			base.Exit (input);
		}	
	
	}
}

