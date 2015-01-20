using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Util;

public class RoleCtrlMessage
{
	protected bool m_IsDestroyed = false;

	protected bool m_isInited = false;
		
	protected RoleBlackBoard m_bbData = null;
	
	protected List<RoleFsmMessage> m_localMsgList = null;
	public List<RoleFsmMessage> LocalMsgList
	{
		get{return m_localMsgList;}
	}

	protected List<RoleFsmMessage> m_synMsgList = null;
	public List<RoleFsmMessage> SynMsgList
	{
		get{return m_synMsgList;}
	}

	protected List<TimeLineMessage> m_waitList = null;
	public List<TimeLineMessage> WaitList
	{
		get{return m_waitList;}
	}

//	protected List<TimeLineMessage> m_runList = null;
//	public List<TimeLineMessage> RunList
//	{
//		get{return m_runList;}
//	}

	//
	protected List<TimeLineMessage> m_moveList = null;
	public List<TimeLineMessage> MoveList
	{
		get{return m_moveList;}
	}

	protected List<TimeLineMessage> m_skillList = null;
	public List<TimeLineMessage> SkillList
	{
		get{return m_skillList;}
	}

	protected List<TimeLineMessage> m_hitList = null;
	public List<TimeLineMessage> HitList
	{
		get{return m_hitList;}
	}

	protected List<TimeLineMessage> m_actionList = null;
	public List<TimeLineMessage> ActionList
	{
		get{return m_actionList;}
	}
	
	public void Initalize(RoleBlackBoard bbData)
	{
		m_isInited = true;
		m_bbData = bbData;

		m_localMsgList = new List<RoleFsmMessage>();
		m_synMsgList = new List<RoleFsmMessage>();

		m_waitList = new List<TimeLineMessage>();
		m_moveList = new List<TimeLineMessage>();
		m_skillList = new List<TimeLineMessage>();
		m_hitList = new List<TimeLineMessage>();
	}

	public void Destroy()
	{
		m_IsDestroyed = true;
	}
	
	public void Update()
	{
		ProduceWaitList();
		CheckWaitList();
	}

	public void AddLocalFsmMsg(RoleFsmMessage fsmMsg)
	{
		fsmMsg.IsLocalMsg = true;
		m_localMsgList.Add(fsmMsg);
	}

	public void AddSynFsmMsg(RoleFsmMessage fsmMsg)
	{
		fsmMsg.IsLocalMsg = false;
		m_synMsgList.Add(fsmMsg);
	}
	
	private void ProduceWaitList()
	{
		//to do
		while(m_localMsgList.Count > 0)
		{
			RoleFsmMessage fsmMsg = m_localMsgList[0];
			m_localMsgList.RemoveAt(0);
			TimeLineMessage tlMsg = TimeLineMessage.GetTLMsgByFsmMsg(fsmMsg);
			m_waitList.Add(tlMsg);
		}

		while(m_synMsgList.Count > 0)
		{
			RoleFsmMessage fsmMsg = m_synMsgList[0];
			m_synMsgList.RemoveAt(0);
			TimeLineMessage tlMsg = TimeLineMessage.GetTLMsgByFsmMsg(fsmMsg);
			m_waitList.Add(tlMsg);
		}
	}
	
	private void CheckWaitList()
	{
		// to do 
	}
	
	public TimeLineMessage GetWaitTLMsgFront()
	{
		if(m_waitList.Count > 0)
		{
			TimeLineMessage tlMsg = m_waitList[0];
			return tlMsg;
		}
		else return null;
	}

	public void RemoveWaitMsg(TimeLineMessage tlMsg)
	{
		if(m_waitList != null)
		{
			m_waitList.Remove(tlMsg);
		}
	}
	
	public void AddRunTLMsg(TimeLineMessage tlMsg)
	{
		switch(tlMsg.GetCmdType)
		{
		case eCommandType.Cmd_Move:
			this.MoveList.Add(tlMsg);
			break;
		case eCommandType.Cmd_Attack:
			this.SkillList.Add(tlMsg);
			break;
		case eCommandType.Cmd_Hit:
			this.HitList.Add(tlMsg);
			break;
		default:
			this.ActionList.Add(tlMsg);
			break;
		}
	}

	public void CleanAllTLMsg()
	{
		MoveList.Clear();
		SkillList.Clear();
		HitList.Clear();
		ActionList.Clear();
	}
	
	private RoleDataRunTime runTimeData
	{
		get
		{
			if(m_bbData != null)
			{
				return m_bbData.DataRunTime;
			}
			else return null;
		}
	}
}
