

public enum eSkillKey
{
	NotUse = 0,
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
	AddEffect,
	AddHitBound,
	AddMagic,
}

public enum eProcessStatus
{
	None,
	Start,
	Run,
	End,
}

//public enum eHitResultType
//{
//	Not_Use = 0,
//	//None = 1,
//	Damage = 2,
//	Force = 3,
//	Die = 4,
//}

public enum eDamageType
{
	Not_Use = 0,
	None = 1,
	Normal = 2,
	Critical = 3,
	Miss = 4,
}

//public enum eDamageForce
//{
//	Not_Use = 0,
//	None,
//	Force_Hit,
//	Force_Back,
//	Force_Up,
//	Force_Down,
//	Force_Stun,
//	Force_Caught ,
//}
