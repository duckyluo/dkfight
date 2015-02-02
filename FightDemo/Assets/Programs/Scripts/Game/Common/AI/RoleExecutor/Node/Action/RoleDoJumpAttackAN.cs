using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;
using Dk.Event;

public class RoleDoJumpAttackAN : RoleDoAction
{
	protected TAttackMessage m_curMsg = null;
	
	public override void Initalize ()
	{
		this.m_name = "DoJumpAttack";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg.GetActionType == eActionType.JumpAttack)
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
		if(m_curMsg.skillKey != eSkillKey.JumpAttack && m_curMsg.skillIndex < 0)
		{
			Debug.LogError(" skill msg error ! key : "+m_curMsg.skillKey + " index : "+m_curMsg.skillIndex );
			Exit(null);
			return;
		}

		GetRunTimeData.ActionType = eActionType.JumpAttack;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;
		GetRunTimeData.IsTrigger = true;

		GetSkillCtrl.StartSkillProcess(m_curMsg.skillKey,m_curMsg.skillIndex);
	}
	
	protected override void DoAction()
	{
		if(GetSkillCtrl.CurSkillStatus == eProcessStatus.Run)
		{
			GetSkillCtrl.UpdateSkillProcess();

			if(GetRunTimeData.PostureType == ePostureType.Pose_None)
			{
				GetRunTimeData.PostureType = ePostureType.Pose_Skill;
				//Shit !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! For res
				//GetTransformCtrl.MoveLimit(new Vector3(0,0.4f,0));
			}

			if(GetSkillCtrl.CurSkillStatus != eProcessStatus.Run)
			{
				ToJumpDown();
			}
		}
	}

	protected override void NextAction ()
	{
		base.NextAction ();
	}
	
	protected void ToJumpDown()
	{
		TMoveMessage moveMsg = new TMoveMessage();
		moveMsg.GetCmdType = eCommandType.Cmd_Move;
		moveMsg.GetActionType = eActionType.JumpDown;
		this.GetMsgCtrl.WaitList.Insert(0,moveMsg);
		
		Exit(null);
	}

	protected override void Exit (DkBtInputParam input)
	{
		m_curMsg = null;
		base.Exit (input);
	}

	protected override void CheckSelfRule()
	{
		base.CheckSelfRule();
	}
}

