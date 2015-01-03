using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleStateMoveCN : RoleBaseSelectorNode
{
	public override void Initalize ()
	{
		this.m_name = "StateMove";
		base.Initalize ();

		this.AddChild(new RoleDoMoveJumpAN());
		this.AddChild(new RoleDoMoveDirectionAN());
		this.AddChild(new RoleDoMoveStopAN());
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		return IsWaitingMove();
	}
	
	protected bool IsWaitingMove()
	{
		if(GetFrontWaitMsg != null)
		{
			if(GetFrontWaitMsg.GetCmdType == eCommandType.Cmd_Move)
			{
				return true;
			}
			else return false;
		}
		else return false;
	}
}