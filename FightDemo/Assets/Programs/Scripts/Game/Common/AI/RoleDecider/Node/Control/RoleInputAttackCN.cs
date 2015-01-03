using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleInputAttackCN : RoleBaseSelectorNode
{
	public override bool Evaluate (DkBtInputParam input)
	{
		if((InputManager.HasAttackKey || InputManager.HasSkillKey) && InputManager.IsConsumeKeyEnalbe && 
		   GetRunTimeData.ActiveChStateEnalbe)
		{
			if(GetRunTimeData.StateType == eStateType.State_Idle || 
			   GetRunTimeData.StateType == eStateType.State_Move || 
			   GetRunTimeData.StateType == eStateType.State_Skill)
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
		this.AddChild(new RoleAskAttackAN());
		this.AddChild(new RoleAskSkillAN());
	}
}

