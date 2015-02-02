using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleInputAttackCN : RoleBaseSelectorNode
{
	public override void Initalize ()
	{
		this.m_name = "InputAttack";
		this.m_isDebug = false;
		base.Initalize ();
		this.AddChild(new RoleAskJumpAttackAN());
		this.AddChild(new RoleAskAttackAN());
		this.AddChild(new RoleAskSkillAN());
	}

	public override bool Evaluate (DkBtInputParam input)
	{
		if((InputManager.HasAttackKey || InputManager.HasSkillKey) && 
		   InputManager.IsConsumeKeyEnalbe && GetRunTimeData.ActiveChStateEnalbe)
		{
			return true;
		}
		else return false;
	}
}

