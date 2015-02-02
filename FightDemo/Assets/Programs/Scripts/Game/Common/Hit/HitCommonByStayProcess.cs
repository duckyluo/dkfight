using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 公共碰撞承受停留伤害(没有固定伤害,根据停留时间,离开碰撞区域不受伤害)
/// </summary>
public class HitCommonByStayProcess : HitMethod
{
	protected float m_startTime = 0f;

	protected float m_timeCount = 0f;
	
	protected int m_hitCount = 0;

	protected SkillHitData m_hitData = null;
	
	public override void TargetEnter (int boundIndex , SkillHitData hitData )
	{
		if(m_status != eProcessStatus.End)
		{
			//Debug.Log(" ====================================== 1");
			m_status = eProcessStatus.Start;
			m_hitData = hitData;
		}
	}

	public override void TargetOut (int boundIndex)
	{
		//Debug.Log(" ====================================== 2");
		m_status = eProcessStatus.None;
	}
	
	public override void Update (float curTime)
	{
		if(m_status == eProcessStatus.Start)
		{
			m_startTime = curTime;
			m_timeCount = 0;

			if(CheckHitMoment(m_hitData))
			{
				m_status = eProcessStatus.Run;
				DoHit();
			}
		}
		else if(m_status == eProcessStatus.Run)
		{
			float passTime = curTime - m_startTime - m_timeCount;
			
			if(passTime >= m_hitData.hitInterval)
			{
				int times = Mathf.FloorToInt(passTime / m_hitData.hitInterval);
				
				m_timeCount += m_hitData.hitInterval * times;

				while(times > 0)
				{
					times--;
					DoHit();
				}
			}
		}
		
		if(m_hitCount == m_hitData.hitTimes )
		{
			m_status = eProcessStatus.End;
		}
	}
	
	public override bool DoHit ()
	{
		if(m_hitCount < m_hitData.hitTimes)
		{
			m_hitCount++;
			
			SceneObjInfo info = m_target.DataInfo;
			
			RoleFsmMessage fsmMsg = new RoleFsmMessage();
			fsmMsg.receiveIndex = info.index;
			fsmMsg.cmdType = eCommandType.Cmd_Hit;
			fsmMsg.curPos = m_target.DataRunTime.CurPos;			
			fsmMsg.actionType = GetActionByHitForce(m_hitData.hitForce, m_hitData.hitSpeed);
			fsmMsg.hitSpeed = m_hitData.hitSpeed;
			fsmMsg.lookDirection = GetLookDirection(m_hitData.hitLook);
			
			if(fsmMsg.lookDirection == eLookDirection.Right)
			{
				fsmMsg.hitSpeed.x = -m_hitData.hitSpeed.x;
			}

			FsmMsgManager.SendFsmMsg(fsmMsg);

			return true;
		}
		else return false;
	}
}

