using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleAskSkillAN : RoleBaseActionNode
{
	protected RoleFsmMessage askMsg = null;

	public override void Initalize ()
	{
		this.m_name = "AskSkill";
		base.Initalize ();
	}

	public override bool Evaluate (DkBtInputParam input)
	{
		if(InputManager.HasSkillKey)
		{
			if(GetRunTimeData.ActionType == eActionType.Jump ||
			   GetRunTimeData.ActionType == eActionType.JumpAttack)
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
			int nextSkillIndex = -1;
			eInputActiveKey inputKey = InputManager.ConsumeActiveKey();
			if(inputKey == eInputActiveKey.Skill_One)
			{
				nextSkillIndex = GetLocalData.GetSkillGroup(eSkillKey.SkillOne).NextSkillIndex();
			}
			else if(inputKey == eInputActiveKey.Skill_Two)
			{
				nextSkillIndex = GetLocalData.GetSkillGroup(eSkillKey.SkillTwo).NextSkillIndex();
			}
			else if(inputKey == eInputActiveKey.Skill_Three)
			{
				nextSkillIndex = GetLocalData.GetSkillGroup(eSkillKey.SkillThree).NextSkillIndex();
			}
			else if(inputKey == eInputActiveKey.Skill_Four)
			{
				nextSkillIndex = GetLocalData.GetSkillGroup(eSkillKey.SkillFour).NextSkillIndex();
			}
			else
			{
				Debug.Log("[error][RoleAskSkillAN] the input thing is error");
				this.m_status = eDkBtRuningStatus.End;
				return;
			}
			

			if(nextSkillIndex >= 0)
			{
				askMsg = new RoleFsmMessage();
				askMsg.receiveIndex = GetRoleBBData.DataInfo.index;
				askMsg.cmdType = eCommandType.Cmd_Attack;
				askMsg.actionType = eActionType.Attack;
				askMsg.lookDirection = GetRunTimeData.LookDirection;
				askMsg.curPos = GetRunTimeData.CurPos;
				askMsg.moveMethod = eMoveMethod.RootPoint;
				askMsg.skillIndex = nextSkillIndex;
				
				GetMsgCtrl.AddLocalFsmMsg(askMsg);
			}
			else
			{
				Debug.Log("[error][RoleAskAttackAN] role "+GetRoleBBData.DataInfo.index+" can not find attack skill");
				this.m_status = eDkBtRuningStatus.End;
			}
		}
		else
		{
			askMsg = null;
			this.m_status = eDkBtRuningStatus.End;
		}
	}
}
