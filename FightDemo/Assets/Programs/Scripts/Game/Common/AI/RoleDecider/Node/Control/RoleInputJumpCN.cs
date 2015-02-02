using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleInputJumpCN : RoleBaseSelectorNode
{
	public override void Initalize ()
	{
		this.m_name = "InputJump";
		this.m_isDebug = false;
		base.Initalize ();
		this.AddChild(new RoleAskJumpAN());
	}

	public override bool Evaluate (DkBtInputParam input)
	{
		if(InputManager.HasJumpKey && InputManager.IsConsumeKeyEnalbe && 
		   GetRunTimeData.ActiveChStateEnalbe)
		{
			return true;
		}
		else return false;
	}
}
