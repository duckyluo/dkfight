using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleNotInputCN : RoleBaseSelectorNode
{
	public override void Initalize ()
	{
		this.m_name = "NotInput";
		this.m_isDebug = false;
		base.Initalize ();
		this.AddChild(new RoleAskStopAN());
	}

	public override bool Evaluate (DkBtInputParam input)
	{
		if(!InputManager.HasDirectKey || !InputManager.HasActiveKey)
		{
			return true;
		}
		else return false;
	}
}
