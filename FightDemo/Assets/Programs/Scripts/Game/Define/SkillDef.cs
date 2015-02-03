

public enum eSkillKey
{
	Not_Use = 0,
	None,
	Attack,
	JumpAttack,
	SkillOne,
	SkillTwo,
	SkillThree,
	SkillFour,
}

public enum eSkillType
{	
	Not_Use = 0,
	None,
	Normal,
	Active,
	Passive,
	Bullet,
}

public enum eEffectMoveMethod
{
	Not_Use = 0,
	None,
	Position,
	Direction,
}

public enum eEffectTarget
{
	Not_Use = 0,
	None,
	RoleSelf,
	RoleTarget,
}

public enum eSkillProcessEventType
{
	Not_Use = 0,
	ChAnimation,
	ChPos,
	ChAlpha,
	ChScale,
	ChAttribute,
	ChCamera,
	AddEffect,
	AddHitBound,
	AddMagic,
}

public enum eDamageType
{
	Not_Use = 0,
	None = 1,
	Normal = 2,
	Critical = 3,
	Miss = 4,
}


[System.Serializable]
public enum eSkillMoveMethod
{
	Not_Use,
	None,
	Translation,
}

[System.Serializable]
public enum eSkillHitLookDirection
{
	Not_Use,
	None,
	OppositeAttackerMove,
	LookAttackerPos,
}

[System.Serializable]
public enum eSkillHitForce
{
	Not_Use = 0,
	None,
	Force_Stun,
	Force_Caught,
}

[System.Serializable]
public enum eHitMethod
{
	Not_Use,
	None,
	HitCommonByNum, 	//公共碰撞承受次数伤害(根据碰撞后绝对间隔时间,离开碰撞区域依然受伤害)
	HitCommonByStay, 	//公共碰撞承受停留伤害(没有固定伤害,根据停留时间,离开碰撞区域不受伤害)
	HitAloneByNum, 	 	//每次碰撞造成不同的伤害(间隔根据碰撞,相同碰撞不重复受伤害)
}

[System.Serializable]
public enum eHitMoment
{
	Not_Use,
	None,
	MoveXPos,
	MoveYPos,
}

