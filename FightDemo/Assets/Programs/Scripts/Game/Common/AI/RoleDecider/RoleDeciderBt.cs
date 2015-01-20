using UnityEngine;
using System.Collections;
using Dk.BehaviourTree;

public class RoleDeciderBt : DkBehaviourTree 
{
	public override void Initalize (System.Object obj)
	{
		base.Initalize(obj);

		this.BBData = obj as RoleBlackBoard;
		this.SetRootNode(new DkBtRootNode(this));
		RoleDeciderDirect direct = new RoleDeciderDirect();
		this.m_rootNode.SetDirectNode(direct);
	}

	public RoleBlackBoard GetRoleBBData
	{
		get{return (RoleBlackBoard)m_bbData;}
	}
}
