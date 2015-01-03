using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;

public class RoleExecutorDirect : RoleBaseDirectNode
{
	public override void Initalize()
	{
		this.m_name = "RoleExecutorDirect";
		base.Initalize();
		this.AddChild(new RoleStateMoveCN());
		this.AddChild(new RoleStateIdleCN());

	}
}
