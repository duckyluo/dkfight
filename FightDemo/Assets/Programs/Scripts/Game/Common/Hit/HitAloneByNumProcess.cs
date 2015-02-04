using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 每次碰撞造成不同的伤害(间隔根据碰撞,相同碰撞不重复受伤害)
/// </summary>
public class HitAloneByNumProcess : HitMethod
{
	protected float m_startTime = 0f;
	
	protected float m_timeCount = 0f;
	
	protected int m_hitCount = 0;
	
	protected SkillHitData m_hitData = null;

	protected List<int> boundList = new List<int>();
	
	public override void TargetEnter (int boundIndex , SkillHitData hitData)
	{
		if(boundList.Contains(boundIndex) == false)
		{
			boundList.Add(boundIndex);
			m_status = eProcessStatus.Start;
			m_hitData = hitData;
		}
	}
	
	public override void Update (float curTime)
	{
		if(m_status == eProcessStatus.Start)
		{
			m_hitCount = 0;
			m_timeCount = 0;
			m_startTime = curTime;

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
			
			SendHitMsg();

			return true;
		}
		else return false;
	}

	protected override void SendHitMsg ()
	{
		base.SendHitMsg ();

		SceneObjInfo info = m_target.DataInfo;
		
		RoleFsmMessage fsmMsg = new RoleFsmMessage();
		fsmMsg.receiveIndex = info.index;
		fsmMsg.cmdType = eCommandType.Cmd_Hit;
		fsmMsg.curPos = m_target.DataRunTime.CurPos;
		fsmMsg.actionType = GetActionByHitForce(m_hitData.hitForce , m_hitData.hitSpeed);
		fsmMsg.hitSpeed = m_hitData.hitSpeed;
		fsmMsg.lookDirection = GetLookDirection(m_hitData.hitLook);
		
		if(fsmMsg.lookDirection == eLookDirection.Right)
		{
			fsmMsg.hitSpeed.x = -m_hitData.hitSpeed.x;
		}
		
		FsmMsgManager.SendFsmMsg(fsmMsg);
	}
}

