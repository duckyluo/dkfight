using UnityEngine;

public class HitTargetProcess
{
	protected RoleBlackBoard m_target = null;

	protected int m_maxHitTimes = 1;
	
	protected float m_hitInterval = 0f;
	
	protected int m_hitCount = 0;
	
	protected float m_timeCount = 0f;

	protected eSkillHitForce m_hitForce = eSkillHitForce.Not_Use;
	
	protected eProcessStatus m_status = eProcessStatus.None;
	public eProcessStatus GetStatus
	{
		get{ return m_status;}
	}
	
	public void Initalize(RoleBlackBoard target , SHitData hitData)
	{
		m_target = target;
		m_maxHitTimes = hitData.hitTimes;
		m_hitInterval = hitData.hitInterval;
		m_hitForce = hitData.hitForce;
		
		m_status = eProcessStatus.Start;
	}

	public void Destroy()
	{
		m_hitCount = 0;
		m_timeCount = 0;
		m_maxHitTimes = 0;
		m_hitInterval = 0;
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

			SceneObjInfo info = m_target.DataInfo;
			//Debug.Log("info "+info.index + " "+info.nick + " "+TimerManager.Instance.GetElapsedTime);
			
			RoleFsmMessage fsmMsg = new RoleFsmMessage();
			
			fsmMsg.receiveIndex = info.index;
			fsmMsg.cmdType = eCommandType.Cmd_Hit;
			fsmMsg.lookDirection = m_target.DataRunTime.LookDirection;
			fsmMsg.curPos = m_target.DataRunTime.CurPos;

			eActionType action = eActionType.ForceHit;
			if(m_hitCount == m_maxHitTimes && m_hitForce != eSkillHitForce.Not_Use)
			{
				action = GetActionByHitForce(m_hitForce);
			}

			if(action == eActionType.ForceHit && m_target.DataRunTime.IsOnAir)
			{
				action = eActionType.ForceFloatHit;
			}

			fsmMsg.actionType = action;
			//Debug.Log("============================ hit !!!!! "+action+" "+m_hitCount+" "+m_maxHitTimes);
			FsmMsgManager.SendFsmMsg(fsmMsg);
		}
	}

	private eActionType GetActionByHitForce(eSkillHitForce force)
	{
		eActionType action = eActionType.ForceHit;
		switch(force)
		{
		case eSkillHitForce.Force_Hit:
			action = eActionType.ForceHit;
			break;
		case eSkillHitForce.Force_Back:
			action = eActionType.ForceBack;
			break;
		case eSkillHitForce.Force_FlyUp:
			action = eActionType.ForceFly;
			break;
		case eSkillHitForce.Force_FallDown:
			action = eActionType.ForceFallDown;
			break;
		default:
			break;
		}

		return action;
	}
}


