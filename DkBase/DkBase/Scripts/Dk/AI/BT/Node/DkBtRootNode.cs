using System;
using System.Collections;
using System.Collections.Generic;

namespace Dk.BehaviourTree
{
	public class DkBtRootNode : DkBtNode
	{
		public DkBtRootNode(DkBehaviourTree bt)
		{
			this.m_bt = bt;
			m_bt.SetRootNode(this);
		}

		private DkBtControlNode m_directNode;
		public void SetDirectNode(DkBtControlNode node)
		{
			if(node != null)
			{
				m_directNode = node;
				m_directNode.Parent = this;
				m_directNode.Initalize();
			}
		}

		public override eDkBtRuningStatus Tick(DkBtInputParam input)
		{
			if(m_directNode != null && m_directNode.Evaluate(input))
			{	
				return m_directNode.Tick(input);
			}
			else return eDkBtRuningStatus.Failed;
		}
	}
}
