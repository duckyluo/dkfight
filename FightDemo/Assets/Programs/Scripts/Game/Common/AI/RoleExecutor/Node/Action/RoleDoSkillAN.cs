using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;
using Dk.Event;

public class RoleDoSkillAN : RoleDoAction
{
	protected TAttackMessage m_curMsg = null;
	
	public override void Initalize ()
	{
		this.m_name = "DoSkill";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg.GetActionType == eActionType.Skill)
		{
			return true;
		}
		else return false;
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		base.Enter(input);

		m_curMsg = GetFrontWaitMsg as TAttackMessage;
		GetMsgCtrl.AddRunTLMsg(GetFrontWaitMsg);
		GetMsgCtrl.RemoveWaitMsg(GetFrontWaitMsg);
	}
	
	protected override void StartAction()
	{
		if( (m_curMsg.skillKey == eSkillKey.SkillOne || m_curMsg.skillKey == eSkillKey.SkillTwo &&
		     m_curMsg.skillKey == eSkillKey.SkillThree || m_curMsg.skillKey == eSkillKey.SkillFour)
		     && m_curMsg.skillIndex >= 0)
		{
			GetRunTimeData.ActionType = eActionType.Skill;
			GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
		
			GetRunTimeData.IsTrigger = true;

			GetSkillCtrl.StartSkillProcess(m_curMsg.skillKey,m_curMsg.skillIndex);
		}
		else
		{
			Debug.LogError(" skill msg error ! key : "+m_curMsg.skillKey + " index : "+m_curMsg.skillIndex );
			Exit(null);
		}
	}
	
	protected override void DoAction()
	{
		if(GetSkillCtrl.CurSkillStatus == eProcessStatus.Run)
		{
			GetSkillCtrl.UpdateSkillProcess();
			
			if(GetSkillCtrl.CurSkillStatus != eProcessStatus.Run)
			{
				Exit(null);
			}
		}
	}

	protected override void NextAction ()
	{
		GetSkillCtrl.FinishSkillProcess();
		base.NextAction ();
	}
	
	protected override void Exit (DkBtInputParam input)
	{
		m_curMsg = null;
		base.Exit (input);
	}
	
	protected override void CheckSelfRule()
	{
		base.CheckSelfRule();
		InputManager.KeyJumpEnalbe = true;
	}
}
