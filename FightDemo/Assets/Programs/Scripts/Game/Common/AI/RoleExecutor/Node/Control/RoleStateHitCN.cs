using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleStateHitCN : RoleBaseSelectorNode
{
	public override void Initalize ()
	{
		this.m_name = "StateHit";
		base.Initalize ();
		this.AddChild(new RoleForceHitAN());
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		return IsWaitingHit();
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		this.GetRunTimeData.StateType = eStateType.State_Hit;
		base.Enter (input);
	}
	
	protected bool IsWaitingHit()
	{
		if(GetFrontWaitMsg != null)
		{
			if(GetFrontWaitMsg.GetCmdType == eCommandType.Cmd_Hit)
			{
				return true;
//				THitMessage msg = GetFrontWaitMsg as THitMessage;
//				if(msg.hitResultType == eHitResultType.Force)
//				{
//					return true;
//				}
//				else return false;
			}
			else return false;
		}
		else return false;
	}
}
