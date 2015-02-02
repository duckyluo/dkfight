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
			if(GetRunTimeData.ActionType == eActionType.Idle ||
			   GetRunTimeData.ActionType == eActionType.Move ||
			   GetRunTimeData.ActionType == eActionType.Skill||
			   GetRunTimeData.ActionType == eActionType.Attack)
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
			eSkillKey skillKey = eSkillKey.None;
			int nextSkillIndex = -1;
			eInputActiveKey inputKey = InputManager.ConsumeActiveKey();

			if(inputKey == eInputActiveKey.Skill_One)
			{
				skillKey = eSkillKey.SkillOne;
				nextSkillIndex = GetLocalData.GetSkillGroup(eSkillKey.SkillOne).NextSkillIndex();
			}
			else if(inputKey == eInputActiveKey.Skill_Two)
			{
				skillKey = eSkillKey.SkillTwo;
				nextSkillIndex = GetLocalData.GetSkillGroup(eSkillKey.SkillTwo).NextSkillIndex();
			}
			else if(inputKey == eInputActiveKey.Skill_Three)
			{
				skillKey = eSkillKey.SkillThree;
				nextSkillIndex = GetLocalData.GetSkillGroup(eSkillKey.SkillThree).NextSkillIndex();
			}
			else if(inputKey == eInputActiveKey.Skill_Four)
			{
				skillKey = eSkillKey.SkillFour;
				nextSkillIndex = GetLocalData.GetSkillGroup(eSkillKey.SkillFour).NextSkillIndex();
			}
			else
			{
				Debug.Log("[error][RoleAskSkillAN] the input thing is error");
				this.m_status = eDkBtRuningStatus.End;
				return;
			}
			
			if( GetSkillCtrl.IsKeyCool(skillKey) )
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
			else if(nextSkillIndex >= 0)
			{
				askMsg = new RoleFsmMessage();
				askMsg.receiveIndex = GetRoleBBData.DataInfo.index;
				askMsg.cmdType = eCommandType.Cmd_Attack;
				askMsg.actionType = eActionType.Attack;
				askMsg.lookDirection = GetRunTimeData.LookDirection;
				askMsg.curPos = GetRunTimeData.CurPos;
				askMsg.skillKey = skillKey;
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
