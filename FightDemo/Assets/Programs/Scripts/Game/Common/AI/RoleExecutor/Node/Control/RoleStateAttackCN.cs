using System;
using System.Collections;
using System.Collections.Generic;
using Dk.BehaviourTree;
using UnityEngine;

public class RoleStateAttackCN : RoleBaseSelectorNode
{
	public override void Initalize ()
	{
		this.m_name = "StateAttack";
		base.Initalize ();
		this.AddChild(new RoleDoSkillAN());
		this.AddChild(new RoleDoAttackAN());
		this.AddChild(new RoleDoJumpAttackAN());
	}
	
	public override bool Evaluate (DkBtInputParam input)
	{
		return IsWaitingAttack();
	}
	
	protected override void Enter (DkBtInputParam input)
	{
		this.GetRunTimeData.StateType = eStateType.State_Attack;
		base.Enter (input);
	}
	
	protected bool IsWaitingAttack()
	{
		if(GetFrontWaitMsg != null)
		{
			if(GetFrontWaitMsg.GetCmdType == eCommandType.Cmd_Attack)
			{
				return true;
			}
			else return false;
		}
		else return false;
	}
}
