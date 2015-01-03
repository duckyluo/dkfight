using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Dk.BehaviourTree;

public class RoleExecutorBt : DkBehaviourTree 
{
	public void Initalize (RoleBlackBoard bbData)
	{
		base.Initalize();
		this.BBData = bbData;
		this.SetRootNode(new DkBtRootNode(this));
		RoleExecutorDirect direct = new RoleExecutorDirect();
		this.m_rootNode.SetDirectNode(direct);
	}

	public RoleBlackBoard GetRoleBBData
	{
		get{return (RoleBlackBoard)m_bbData;}
	}
}