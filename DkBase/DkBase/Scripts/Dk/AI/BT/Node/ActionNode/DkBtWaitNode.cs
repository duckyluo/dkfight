using System;

using System.Collections;
using System.Collections.Generic;

namespace Dk.BehaviourTree
{
	public class DkBtWaitNode : DkBtActionNode
	{
		protected float m_waitTime = 0;

		protected float m_startTime = 0;

		public DkBtWaitNode(float waitTime)
		{
			m_waitTime = waitTime;
		}
		
//		protected void beginWait()
//		{
//			m_startTime = this.m_bt.GetCurMilliTime;
//		}
//
//		public bool IsTimeOut
//		{
//			get
//			{
//				if(( this.m_bt.GetCurMilliTime - m_startTime) >= m_waitTime)
//				{
//					return true;
//				}
//				else return false;
//			}
//		}

		protected override void Enter(DkBtInputParam input)
		{
			base.Enter(input);
			//beginWait();
		}

	}
}

