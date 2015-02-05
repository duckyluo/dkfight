using UnityEngine;
using System;

public class HitMethod
{
	protected RoleBlackBoard m_self = null;
	protected RoleBlackBoard m_target = null;
	protected eProcessStatus m_status = eProcessStatus.None;

	public eProcessStatus GetStatus
	{
		get{return m_status;}
	}
	
	virtual public void Initalize(RoleBlackBoard target , RoleBlackBoard self)
	{
		m_self = self;
		m_target = target;
		m_status = eProcessStatus.None;
	}
	
	virtual public void Destroy()
	{
		m_self = null;
		m_target = null;
		m_status = eProcessStatus.End;
	}
	
	virtual public void TargetEnter(int boundIndex , SkillHitData hitData){}
	
	virtual public void TargetOut(int boundIndex){}
	
	virtual public void Update(float timeCount){}
	
	virtual public bool DoHit(){ return false;}

	protected eLookDirection GetLookDirection(SkillHitData hitData)
	{
		eLookDirection lookDirection = m_target.DataRunTime.LookDirection;
		if(hitData.hitLook == eSkillHitLookDirection.LookAttackerPos)
		{
			lookDirection = m_target.DataRunTime.GetLookDirectionToPos(m_self.DataRunTime.CurPos);
		}
		else if(hitData.hitLook == eSkillHitLookDirection.OppositeAttackerMove)
		{
			if(m_self.DataRunTime.MoveDirection == eMoveDirection.Right)
			{
				lookDirection = eLookDirection.Left;
			}
			else
			{
				lookDirection = eLookDirection.Right;
			}
		}
		else if(hitData.hitLook == eSkillHitLookDirection.OppositeAttackerLook)
		{
			if(m_self.DataRunTime.LookDirection == eLookDirection.Right)
			{
				lookDirection = eLookDirection.Left;
			}
			else
			{
				lookDirection = eLookDirection.Right;
			}
		}

		return lookDirection;
	}

	protected Vector3 GetHitSpeedBy(SkillHitData hitData)
	{
		Vector3 hitSpeed = Vector3.zero;
		if(hitData.hitForce == eSkillHitForce.Force_Caught)
		{
			Debug.LogError("Not Yet Now !!!!!!!!!!!!!!!!!!!!!!!!!!");
		}
		else if(hitData.hitForce == eSkillHitForce.Force_Stun)
		{
			Debug.LogError("Not Yet Now !!!!!!!!!!!!!!!!!!!!!!!!!!");
		}
		else if(hitData.hitForce == eSkillHitForce.Force_ComeUp)
		{
			Vector3 endPos = Vector3.zero;
			if(m_self.DataRunTime.LookDirection == eLookDirection.Right)
			{
				endPos = m_self.DataRunTime.CurPos + new Vector3(1.5f,0,0);
			}
			else
			{
				endPos = m_self.DataRunTime.CurPos - new Vector3(1.5f,0,0);
			}
			hitSpeed = endPos - m_target.DataRunTime.CurPos;
		}
		else 
		{
			hitSpeed = hitData.hitSpeed;
			eLookDirection lool = GetLookDirection(hitData);
			if(lool == eLookDirection.Right)
			{
				hitSpeed.x = -hitData.hitSpeed.x;
			}
		}

		return hitSpeed;
	}
	
	protected eActionType GetActionByHitForce(SkillHitData hitData)
	{
		eActionType action = eActionType.ForceHit;

		if(hitData.hitForce == eSkillHitForce.Force_Caught)
		{
			Debug.LogError("Not Yet Now !!!!!!!!!!!!!!!!!!!!!!!!!!");
			return action;
		}
		else if(hitData.hitForce == eSkillHitForce.Force_Stun)
		{
			Debug.LogError("Not Yet Now !!!!!!!!!!!!!!!!!!!!!!!!!!");
			return action;
		}
		else if(hitData.hitForce == eSkillHitForce.Force_Motion)
		{
			Debug.LogError("Not Yet Now !!!!!!!!!!!!!!!!!!!!!!!!!!");
			action = eActionType.ForceMotion;
		}
		else if(hitData.hitForce == eSkillHitForce.Force_ComeUp)
		{
			action = eActionType.ForceMotion;
		}
		else
		{
			if(m_target.DataRunTime.IsOnAir)
			{
				if(hitData.hitSpeed.sqrMagnitude == 0)
				{
					action = eActionType.ForceFloatHit;
				}
				else if(hitData.hitSpeed.y >= 0)
				{
					action = eActionType.ForceFly;
				}
				else if(hitData.hitSpeed.y < 0)
				{
					action = eActionType.ForceFallDown;
				}
			}
			else
			{
				if(hitData.hitSpeed.y > 0)
				{
					action = eActionType.ForceFly;
				}
				else if(hitData.hitSpeed.y < 0)
				{
					action = eActionType.ForceDown;
				}
				else 
				{
					action = eActionType.ForceHit;
				}
			}
		}
		return action;
	}

	protected Boolean CheckHitMoment(SkillHitData hitData)
	{
		bool bol = false;
		if(hitData.hitMoment == eHitMoment.Not_Use || hitData.hitMoment == eHitMoment.None)
		{
			bol = true;
		}
		else if(hitData.hitMoment == eHitMoment.MoveXPos)
		{
			if(m_self.DataRunTime.MoveDirection == eMoveDirection.Right)
			{
				if(m_self.DataRunTime.CurPos.x > m_target.DataRunTime.CurPos.x)
				{
					bol = true;
				}
			}
			else if(m_self.DataRunTime.MoveDirection == eMoveDirection.Left)
			{
				if(m_self.DataRunTime.CurPos.x < m_target.DataRunTime.CurPos.x)
				{
					bol = true;
				}
			}
		}

		if(hitData.hitTimeScale == SkillTimeScaleMoment.HitMoment)
		{
			TimerManager.Instance.ChangeTimeScale(0.08f,0.15f);
		}

		return bol;
	}

	virtual	protected void SendHitMsg(SkillHitData hitData)
	{
		if(hitData.hitSound != SoundDef.Not_Use)
		{
			SoundManager.PlaySound(hitData.hitSound);
		}
	}
}


