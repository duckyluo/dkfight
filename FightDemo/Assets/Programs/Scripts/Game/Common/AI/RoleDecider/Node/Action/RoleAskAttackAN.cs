using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleAskAttackAN : RoleAskActionNode
{
	protected RoleFsmMessage askMsg = null;

	public override bool Evaluate (DkBtInputParam input)
	{
		if(InputManager.HasAttackKey)
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
