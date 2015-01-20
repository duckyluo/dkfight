using UnityEngine;
//using System;

public class HitTargetProcess
{
	//protected GameObject m_target = null;
	protected RoleBlackBoard m_target = null;
	//protected RoleDataLocal m_localData = null;

	protected int m_maxHitTimes = 1;
	
	protected float m_hitInterval = 0f;
	
	protected int m_hitCount = 0;
	
	protected float m_timeCount = 0f;
	
	protected eProcessStatus m_status = eProcessStatus.None;
	public eProcessStatus GetStatus
	{
		get{ return m_status;}
	}
	
	public void Initalize(RoleBlackBoard target , int maxHitTimes , float hitInterval)
	{
		m_target = target; //target.GetComponent<RoleDataLocal>();
		m_maxHitTimes = maxHitTimes;
		m_hitInterval = hitInterval;
		
		m_status = eProcessStatus.Start;
	}

	public void Destroy()
	{
		m_target = null;
	}
	
	public void Update()
	{
		if(m_status == eProcessStatus.Start)
		{
			m_status = eProcessStatus.Run;
			DoHit();
		}
		else if(m_status == eProcessStatus.Run)
		{
			CheckTime();
		}
		
		if(m_hitCount == m_maxHitTimes )
		{
			m_status = eProcessStatus.End;
		}
	}
	
	private void CheckTime()
	{
		m_timeCount += TimerManager.Instance.GetDeltaTime;
		if(m_timeCount >= m_hitInterval)
		{
			int times = Mathf.FloorToInt(m_timeCount/m_hitInterval);
	
			m_timeCount -= m_hitInterval*times;
			m_timeCount = m_timeCount < 0 ? 0 : m_timeCount;

			//Debug.Log(m_hitInterval + " " + times + "  ");

			while(times > 0)
			{
				times--;
				DoHit();
			}
		}
	}
	
	private void DoHit()
	{
		if(m_hitCount < m_maxHitTimes)
		{
			m_hitCount++;
			
			//RoleDataLocal localData = m_target.GetComponent<RoleDataLocal>();
			SceneObjInfo info = m_target.DataInfo;
			//Debug.Log("info "+info.index + " "+info.nick + " "+TimerManager.Instance.GetElapsedTime);
			
			RoleFsmMessage fsmMsg = new RoleFsmMessage();
			
			fsmMsg.receiveIndex = info.index;
			fsmMsg.cmdType = eCommandType.Cmd_Hit;
			fsmMsg.actionType = eActionType.ForceHit;
			fsmMsg.lookDirection = m_target.DataRunTime.LookDirection;
			fsmMsg.curPos = m_target.DataRunTime.CurPos;
			
			FsmMsgManager.SendFsmMsg(fsmMsg);
		}
	}
}


