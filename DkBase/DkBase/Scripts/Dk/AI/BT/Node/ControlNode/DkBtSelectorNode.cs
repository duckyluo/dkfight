using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dk.BehaviourTree
{
	/// <summary>
	/// Or 
	/// </summary>
	public class DkBtSelectorNode : DkBtControlNode
	{	
		protected DkBtNode m_curTarget = null;

		protected override void Enter (DkBtInputParam input)
		{
			base.Enter (input);
		}

		protected override void Exectue (DkBtInputParam input)
		{
			if(m_curTarget != null)
			{
				m_curTarget.Tick(input);
				if(m_curTarget.IsFailed)
				{
					m_curTarget.Finish();
					m_curTarget = null;
					this.m_status = eDkBtRuningStatus.End;
				}
				else if(m_curTarget.IsBusy)
				{
					this.m_status = eDkBtRuningStatus.Running;
				}
				else
				{
					m_curTarget = null;
					this.m_status = eDkBtRuningStatus.End;
				}
			}
			else if(this.m_status == eDkBtRuningStatus.Running)
			{
				this.m_status = eDkBtRuningStatus.End;
				int count = 0;
				for( int i = 0 ; i < this.m_childNodeList.Count ; i++)
				{
					m_curTarget = m_childNodeList[i];
					if(m_curTarget.Evaluate(input))
					{
						eDkBtRuningStatus result = m_curTarget.Tick(input);
						if(m_curTarget.IsFailed)
						{
							m_curTarget = null;
						}
						else if(m_curTarget.IsBusy)
						{
							this.m_status = eDkBtRuningStatus.Running;
						}
						else
						{
							m_curTarget = null;
						}
						break;
					}
					else
					{
						m_curTarget = null;
						count++;
						if(count == this.m_childNodeList.Count)
						{
							this.m_status = eDkBtRuningStatus.Failed; // nothing can find to do
						}
					}
				}
			}
		}

		protected override void Exit (DkBtInputParam input)
		{
			base.Exit (input);
		}
	}
}
