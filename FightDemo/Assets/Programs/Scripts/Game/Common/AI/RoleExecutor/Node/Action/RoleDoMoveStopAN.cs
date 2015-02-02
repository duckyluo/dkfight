using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

/// <summary>
/// Role do stop.
/// </summary>
public class RoleDoMoveStopAN : RoleDoAction
{
	protected TMoveMessage m_curMsg = null;
	
	public override void Initalize ()
	{
		this.m_name = "DoStop";
		base.Initalize ();
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetFrontWaitMsg.GetActionType == eActionType.Stop)
		{
			return true;
		}
		else return false;
	}

	protected override void Enter (DkBtInputParam input)
	{
		base.Enter(input);

		m_curMsg = GetFrontWaitMsg as TMoveMessage;
		GetMsgCtrl.AddRunTLMsg(GetFrontWaitMsg);
		GetMsgCtrl.RemoveWaitMsg(GetFrontWaitMsg);
	}
	
	protected override void StartAction()
	{
		GetRunTimeData.ActionType = eActionType.Stop;
		GetRunTimeData.MoveMethod = eMoveMethod.Gravity;
		GetRunTimeData.LookDirection = m_curMsg.GetLookDirection;

		GetRunTimeData.MoveEnable = true;
		GetRunTimeData.ActiveChStateEnalbe = true;
		GetRunTimeData.PassiveChStateEnalbe = true;
		GetRunTimeData.UseGravity = eUseGravity.Yes;

		float dis = Vector3.Distance(m_curMsg.GetCurPos,GetRunTimeData.CurPos);
		if(dis == 0)
		{
			Exit(null);
		}
	}

	protected override void DoAction()
	{
		//to do
		Debug.Log("[error]some thing error, des "+ m_curMsg.GetCurPos + " , cur "+GetRunTimeData.CurPos);

		Vector3 motion = m_curMsg.GetCurPos - GetRunTimeData.CurPos;
		GetTransformCtrl.MoveLimit(motion);

		Exit(null);
	}

	protected override void NextAction ()
	{
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
