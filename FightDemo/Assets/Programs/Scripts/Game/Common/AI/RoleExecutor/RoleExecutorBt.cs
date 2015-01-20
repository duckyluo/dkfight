using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Dk.BehaviourTree;

public class RoleExecutorBt : DkBehaviourTree 
{
	public override void Initalize (System.Object obj)
	{
		base.Initalize(obj);
		this.BBData = obj as RoleBlackBoard;
		this.SetRootNode(new DkBtRootNode(this));
		RoleExecutorDirect direct = new RoleExecutorDirect();
		this.m_rootNode.SetDirectNode(direct);
	}

	public RoleBlackBoard GetRoleBBData
	{
		get{return (RoleBlackBoard)m_bbData;}
	}
}