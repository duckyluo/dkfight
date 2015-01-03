using UnityEngine;
using System.Collections;
using Dk.BehaviourTree;

public class RoleDeciderBt : DkBehaviourTree 
{
	public void Initalize (RoleBlackBoard bbData)
	{
		base.Initalize();

		this.BBData = bbData;
		this.SetRootNode(new DkBtRootNode(this));
		RoleDeciderDirect direct = new RoleDeciderDirect();
		this.m_rootNode.SetDirectNode(direct);
	}

	public RoleBlackBoard GetRoleBBData
	{
		get{return (RoleBlackBoard)m_bbData;}
	}
}
