using System;
using System.Collections;
using System.Collections.Generic;

namespace Dk.BehaviourTree
{
	public class DkBtNode
	{
		protected bool isRunning = true;

		protected string m_name = "";
		public string Name
		{
			get{return m_name;}
		}

		protected int m_weight = 0;
		public int Weight
		{
			get{return m_weight;}
		}

		protected DkBtNode m_parentNode = null;
		public DkBtNode Parent
		{
			set{this.m_parentNode = value;}
			get{return this.m_parentNode;}
		}

		protected DkBehaviourTree m_bt = null;
		public DkBehaviourTree BT
		{
			set{this.m_bt = value;}
			get{return this.m_bt;}
		}
		
		protected eDkBtRuningStatus m_status = eDkBtRuningStatus.None;
		public eDkBtRuningStatus Status
		{
			get{ return m_status; }
		}

		protected DkBtPrecondition externalPreconditon;
		public DkBtPrecondition ExternalPrecondtion
		{
			set{ externalPreconditon = value; }
		}
		
		virtual	public bool Evaluate(DkBtInputParam input)
		{
			return (externalPreconditon == null || externalPreconditon.IsTure(input)) ;
		}

		virtual public void Pause()
		{
			isRunning = false;
		}

		virtual public void Resume()
		{
			isRunning = true;
		}

		virtual public void Initalize()
		{
			this.m_bt = this.m_parentNode.BT;
		}

		virtual	public eDkBtRuningStatus Tick(DkBtInputParam input)
		{
			if(this.m_status == eDkBtRuningStatus.None || this.m_status == eDkBtRuningStatus.Start)
			{
				this.Enter(input);
			}
			
			if(this.m_status == eDkBtRuningStatus.Running)
			{
				this.Exectue(input);
			}
			
			if(this.m_status == eDkBtRuningStatus.End)
			{
				this.Exit(input);
			}
			else if(this.m_status == eDkBtRuningStatus.Failed)
			{
				this.Failure(input);
			}

			return this.m_status;
		}

		protected virtual void Enter(DkBtInputParam input)
		{
			this.m_status = eDkBtRuningStatus.Running;
		}
		
		protected virtual void Exectue(DkBtInputParam input)
		{
			this.m_status = eDkBtRuningStatus.End;
		}
		
		protected virtual void Exit(DkBtInputParam input)
		{
			this.m_status = eDkBtRuningStatus.None;
		}

		protected virtual void Failure(DkBtInputParam input)
		{
			//this.m_status = eDkBtRuningStatus.None;
		}

		public virtual void Finish()
		{
			this.m_status = eDkBtRuningStatus.None;
		}
		
		public bool IsBusy
		{
			get{return (this.m_status != eDkBtRuningStatus.None && this.m_status != eDkBtRuningStatus.Failed);}
		}

		public bool IsFree
		{
			get{return (this.m_status == eDkBtRuningStatus.None);}
		}

		public bool IsFailed
		{
			get{return (this.m_status == eDkBtRuningStatus.Failed);}
		}
				
	}
}

