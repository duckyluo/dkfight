using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

/// <summary>
/// Role do hurt
/// </summary>
public class RoleDoHurtAN : RoleBaseActionNode
{
	public override bool Evaluate (DkBtInputParam input)
	{
		if(GetMsgCtrl != null && GetMsgCtrl.HitList.Count > 0)
		{
			return true;
		}
		else return false;
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		base.Enter(input);
	}
	
	protected override void Exectue (DkBtInputParam input)
	{
		checkHitMsg();
		base.Exectue(input);
	}

	protected void checkHitMsg()
	{

	}
}
