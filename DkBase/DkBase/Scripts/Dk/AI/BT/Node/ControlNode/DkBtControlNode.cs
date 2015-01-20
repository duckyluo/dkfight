using System;
using System.Collections;
using System.Collections.Generic;

namespace Dk.BehaviourTree
{
	/// <summary>
	/// ControlNode
	/// </summary>
	public class DkBtControlNode : DkBtNode
	{
		protected List<DkBtNode> m_childNodeList = new List<DkBtNode>();
						
		virtual	public void AddChild(DkBtNode node)
		{
			if(!m_childNodeList.Contains(node))
			{
				bool isInsert = false;
				for(int i = 0; i < m_childNodeList.Count; i++)
				{
					DkBtNode childItem = m_childNodeList[i];
					if(node.Weight > childItem.Weight)
					{
						m_childNodeList.Insert(i,node);
						isInsert = true;
						break;
					}
				}
				
				if(!isInsert)
				{
					m_childNodeList.Insert(m_childNodeList.Count,node);
				}
				
				node.Parent = this;
				node.Initalize();
			}
		}

		public void SortNode()
		{
			m_childNodeList.Sort((a,b) =>
			{
				if(a.Weight < b.Weight)
				{
					return -1;
				}
				else if(a.Weight > b.Weight)
				{
					return 1;
				}
				else return 0;
			});
		}

		public override void Finish ()
		{
			base.Finish();

			foreach(DkBtNode item in m_childNodeList)
			{
				item.Finish();
			}
		}
	}
}



