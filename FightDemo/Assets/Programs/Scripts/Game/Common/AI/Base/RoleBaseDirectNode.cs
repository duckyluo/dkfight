using Dk.BehaviourTree;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RoleBaseDirectNode : DkBtControlNode
{
	protected DkBtNode m_curTarget = null;

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
			else if(m_curTarget.IsFree)
			{
				m_curTarget = null;
				this.m_status = eDkBtRuningStatus.Running;
			}
			else
			{
				this.m_status = eDkBtRuningStatus.Running;
			}
		}
		
		if(m_curTarget == null && this.m_status == eDkBtRuningStatus.Running)
		{
			this.m_status = eDkBtRuningStatus.End;
			
			int index = 0;
			int count = 0;

			while(index < this.m_childNodeList.Count)
			{
				DkBtNode node = m_childNodeList[index];
				if(node.Evaluate(input))
				{
					count++;
					if(count > 100) //to prevent endless loop
					{
						Debug.Log("[error]" + this.m_name +" Are u kidding ? deadLoop ! index : "+index);
					}

					node.Tick(input);
					if(node.IsBusy)
					{
						this.m_status = eDkBtRuningStatus.Running;
						m_curTarget = node;
						break;
					}
					else if(node.IsFailed)
					{
						node.Finish();
						this.m_status = eDkBtRuningStatus.End;
						break;
					}
					else
					{
						index = 0;
						continue;
					}
				}
				else
				{
					index++;
				}
			}
		}
	}
}

