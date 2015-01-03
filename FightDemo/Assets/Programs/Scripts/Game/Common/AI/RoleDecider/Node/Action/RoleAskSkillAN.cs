using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleAskSkillAN : RoleBaseActionNode
{
	protected RoleFsmMessage msg = null;
	
	public override bool Evaluate (DkBtInputParam input)
	{
		if(InputManager.HasSkillKey)
		{
			return true;
		}
		else return false;
	}
	
	public override eDkBtRuningStatus Tick (DkBtInputParam input)
	{
		return base.Tick (input);
	}
}
