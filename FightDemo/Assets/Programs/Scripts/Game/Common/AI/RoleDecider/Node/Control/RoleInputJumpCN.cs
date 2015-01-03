using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleInputJumpCN : RoleBaseSelectorNode
{
	public override bool Evaluate (DkBtInputParam input)
	{
		if(InputManager.HasJumpKey && InputManager.IsConsumeKeyEnalbe && GetRunTimeData.ActiveChStateEnalbe)
		{
			if(GetRunTimeData.StateType == eStateType.State_Idle || 
			   GetRunTimeData.StateType == eStateType.State_Move)
			{
				return true;
			}
			else return false;
		}
		else return false;
	}
	
	public override void Initalize ()
	{
		base.Initalize ();
		this.AddChild(new RoleAskJumpAN());
	}
}