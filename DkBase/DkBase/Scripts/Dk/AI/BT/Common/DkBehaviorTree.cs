using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

namespace Dk.BehaviourTree
{
	public class DkBehaviourTree
	{
//		protected Stopwatch m_stopwatch = new Stopwatch();
//		public float GetCurMilliTime
//		{
//			get
//			{
//				if(m_stopwatch != null)
//				{
//					return m_stopwatch.ElapsedMilliseconds;
//				}
//				else return 0;
//			}
//		}
		
		protected bool inited = false;

		protected string m_name = "";
		public string Name
		{
			set{m_name = value;}
			get{return m_name;}
		}

		protected bool m_paused = false;
		public bool Paused
		{
			get{return m_paused;}
		}
		
		protected eDkBtRuningStatus m_status;
		public eDkBtRuningStatus Status
		{
			get{return m_status;}
		}

		protected DkBtRootNode m_rootNode;
		public void SetRootNode(DkBtRootNode node)
		{
			m_rootNode = node;
		}
		
		protected DkBlackboard m_bbData;
		public DkBlackboard BBData
		{
			set{m_bbData = value;}
			get{return m_bbData;}
		}
		
		public virtual void Initalize(Object obj)
		{
			//resetTimer();
			inited = true;
		}

//		protected virtual void resetTimer()
//		{
//			m_stopwatch.Reset();
//			m_stopwatch.Start();
//		}
	
		public virtual void Destroy()
		{
//			if(m_stopwatch != null)
//			{
//				m_stopwatch.Stop();
//				m_stopwatch = null;
//			}

			m_paused = true;
			m_rootNode = null;
			m_bbData = null;
		}

		public void Pause()
		{
			m_paused = true;
			//m_stopwatch.Stop();
		}

		public void Resume()
		{
			m_paused = false;
			//m_stopwatch.Restart();
		}

		public bool IsRunning
		{
			get
			{
				if(this.inited && m_paused == false)
				{
					return true;
				}
				else return false;
			}
		}

		public void Update(DkBtInputParam inputParam)
		{
			if(!this.inited || this.m_paused || this.m_rootNode == null)
			{
				return;
			}

			m_status = m_rootNode.Tick(inputParam);
		}

	}
}
