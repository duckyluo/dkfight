using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleStateIdleCN : RoleBaseSelectorNode
{
	public override void Initalize ()
	{
		this.m_name = "StateIdle";
		this.m_isDebug = false;
		base.Initalize ();
		this.AddChild(new RoleJoinLieAN());
		this.AddChild(new RoleDoShakeAN());
		this.AddChild(new RoleDoIdleAN());
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		return true;
	}

	protected override void Enter (DkBtInputParam input)
	{
		this.GetRunTimeData.StateType = eStateType.State_Idle;
		base.Enter (input);
	}
}