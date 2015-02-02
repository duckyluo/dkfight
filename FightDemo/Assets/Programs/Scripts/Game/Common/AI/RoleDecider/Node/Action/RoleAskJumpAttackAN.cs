using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleAskJumpAttackAN : RoleAskAction
{
	protected RoleFsmMessage askMsg = null;
	
	public override void Initalize ()
	{
		this.m_name = "AskJumpAttack";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if( GetRunTimeData.ActionType == eActionType.Jump || 
		   	GetRunTimeData.ActionType == eActionType.JumpDown ||
			GetRunTimeData.ActionType == eActionType.JumpAttack )
		{
			if(InputManager.HasAttackKey && GetRunTimeData.JumpAtkEnable)
			{
				return true;
			}
			else return false;
		}
		else return false;
	}
	
	protected override void Exectue (DkBtInputParam input)
	{
		if(askMsg == null)
		{
			if(InputManager.ConsumeActiveKey() != eInputActiveKey.Attack)
			{
				Debug.Log("[error][RoleAskJumpAttackAN] the input thing is error");
				this.m_status = eDkBtRuningStatus.End;
				return;
			}

			if(GetSkillCtrl.IsKeyCool(eSkillKey.JumpAttack))
			{
				Debug.Log("[info] : "+eSkillKey.JumpAttack + " is cool ");
				this.m_status = eDkBtRuningStatus.End;
				return;
			}
			else
			{
				int nextSkillIndex = GetLocalData.GetSkillGroup(eSkillKey.JumpAttack).NextSkillIndex();
				if(nextSkillIndex >= 0)
				{
					askMsg = new RoleFsmMessage();
					askMsg.receiveIndex = GetRoleBBData.DataInfo.index;
					askMsg.cmdType = eCommandType.Cmd_Attack;
					askMsg.actionType = eActionType.JumpAttack;
					askMsg.lookDirection = GetRunTimeData.LookDirection;
					askMsg.curPos = GetRunTimeData.CurPos;
					askMsg.skillKey = eSkillKey.JumpAttack;
					askMsg.skillIndex = nextSkillIndex;
					
					GetMsgCtrl.AddLocalFsmMsg(askMsg);
				}
				else
				{
					Debug.Log("[error][RoleAskAttackAN] role "+GetRoleBBData.DataInfo.index+" can not find attack skill");
					this.m_status = eDkBtRuningStatus.End;
				}
			}

		}
		else
		{
			askMsg = null;
			this.m_status = eDkBtRuningStatus.End;
		}
	}
}
