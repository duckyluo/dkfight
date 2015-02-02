using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleAskAttackAN : RoleAskAction
{
	protected RoleFsmMessage askMsg = null;
	
	public override void Initalize ()
	{
		this.m_name = "AskAttack";
		base.Initalize ();
	}

	public override bool Evaluate (DkBtInputParam input)
	{
		if(InputManager.HasAttackKey)
		{
			if( GetRunTimeData.ActionType == eActionType.Jump ||
			   GetRunTimeData.ActionType == eActionType.JumpAttack )
			{
				return false;
			}
			else return true;
		}
		else return false;
	}

	protected override void Exectue (DkBtInputParam input)
	{
		if(askMsg == null)
		{
			if(InputManager.ConsumeActiveKey() != eInputActiveKey.Attack)
			{
				Debug.Log("[error][RoleAskAttackAN] the input thing is error");
				this.m_status = eDkBtRuningStatus.End;
				return;
			}

			if(GetSkillCtrl.IsKeyCool(eSkillKey.Attack))
			{
				if(GetRunTimeData.StateType == eStateType.State_Attack)
				{
					this.m_status = eDkBtRuningStatus.End;
				}
				else
				{
					askMsg = new RoleFsmMessage();
					askMsg.receiveIndex = GetRoleBBData.DataInfo.index;
					askMsg.cmdType = eCommandType.Cmd_Action;
					askMsg.actionType = eActionType.Shake;
					askMsg.lookDirection = GetRunTimeData.LookDirection;
					askMsg.curPos = GetRunTimeData.CurPos;
					
					GetMsgCtrl.AddLocalFsmMsg(askMsg);
				}
			}
			else
			{
				int nextSkillIndex = GetLocalData.GetSkillGroup(eSkillKey.Attack).NextSkillIndex();
				if(nextSkillIndex >= 0)
				{
					askMsg = new RoleFsmMessage();
					askMsg.receiveIndex = GetRoleBBData.DataInfo.index;
					askMsg.cmdType = eCommandType.Cmd_Attack;
					askMsg.actionType = eActionType.Attack;
					askMsg.lookDirection = GetRunTimeData.LookDirection;
					askMsg.curPos = GetRunTimeData.CurPos;
					askMsg.skillKey = eSkillKey.Attack;
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
