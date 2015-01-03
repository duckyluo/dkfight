using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleDeciderDirect : RoleBaseDirectNode
{
	public override void Initalize()
	{
		this.m_name = "RoleDeciderDirect";
		base.Initalize();

		this.AddChild(new RoleInputAttackCN());
		this.AddChild(new RoleInputJumpCN());
		this.AddChild(new RoleInputMoveCN());
		this.AddChild(new RoleNotInputCN());
	}
}
