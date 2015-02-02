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

	protected eLookDirection GetLookDirection(eSkillHitLookDirection hitLook)
	{
		eLookDirection lookDirection = m_target.DataRunTime.LookDirection;
		if(hitLook == eSkillHitLookDirection.LookAttackerPos)
		{
			lookDirection = m_target.DataRunTime.GetLookDirectionToPos(m_self.DataRunTime.CurPos);
		}
		else if(hitLook == eSkillHitLookDirection.OppositeAttackerLook)
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
	
	protected eActionType GetActionByHitForce(eSkillHitForce force , Vector3 hitSpeed)
	{
		eActionType action = eActionType.ForceHit;

		if(force == eSkillHitForce.Force_Caught)
		{
			Debug.LogError("Not Yet Now !!!!!!!!!!!!!!!!!!!!!!!!!!");
			return action;
		}
		else if(force == eSkillHitForce.Force_Stun)
		{
			Debug.LogError("Not Yet Now !!!!!!!!!!!!!!!!!!!!!!!!!!");
			return action;
		}

		if(m_target.DataRunTime.IsOnAir)
		{
			if(hitSpeed.y >= 0 && hitSpeed.magnitude != 0)
			{
				action = eActionType.ForceFly;
			}
			else if(hitSpeed.y < 0)
			{
				action = eActionType.ForceFallDown;
			}
			else
			{
				action = eActionType.ForceFloatHit;
			}
		}
		else
		{
			if(hitSpeed.y > 0)
			{
				action = eActionType.ForceFly;
			}
			else if(hitSpeed.y < 0)
			{
				action = eActionType.ForceDown;
			}
			else 
			{
				action = eActionType.ForceHit;
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
//			else
//			{
//				Debug.Log("Skill MoveDirection Error !!!!!!!!!!!!!!!!!!!!!!!!!! ===================");
//			}
		}
		return bol;
	}
}

//[System.Serializable]
//public enum eHitMethod
//{
//	Not_Use,
//	None,
//	HitCommonByNum, 	//公共碰撞承受次数伤害(根据碰撞后绝对间隔时间,离开碰撞区域依然受伤害)
//	HitCommonByStay, 	//公共碰撞承受停留伤害(没有固定伤害,根据停留时间,离开碰撞区域不受伤害)
//	HitAloneByNum, 	 	//每次碰撞造成不同的伤害(间隔根据碰撞,相同碰撞不重复受伤害)
//}


