using UnityEngine;

public class CharacterUtil
{

	public static eMoveDirection GetMoveDirectionBySpeed(float xSpeed)
	{
		eMoveDirection moveDirection = eMoveDirection.None;
		if(xSpeed > 0)
		{
			moveDirection = eMoveDirection.Right;
		}
		else if(xSpeed < 0)
		{
			moveDirection = eMoveDirection.Left;
		}
		return moveDirection;
	}
	
	public static eMoveDirection GetMoveDirectionByLook(eLookDirection lookDirection)
	{
		eMoveDirection moveDirection = eMoveDirection.None;
		if(lookDirection == eLookDirection.Right)
		{
			moveDirection = eMoveDirection.Right;
		}
		else if(lookDirection == eLookDirection.Left)
		{
			moveDirection = eMoveDirection.Left;
		}
		return moveDirection;
	}
	
	public static eMoveDirection GetMoveDirectionByJump(eJumpDirection jumpDirection)
	{
		eMoveDirection moveDirection = eMoveDirection.None;
		if(jumpDirection == eJumpDirection.Right)
		{
			moveDirection = eMoveDirection.Right;
		}
		else if(jumpDirection == eJumpDirection.Left)
		{
			moveDirection = eMoveDirection.Left;
		}
		return moveDirection;
	}

	public static float GetXSpeed(float speed , float resistance , eMoveDirection moveDirection)
	{
		if(speed == 0)
		{
			return 0;
		}
		
		float xSpeed = Mathf.Abs(speed);
		
		if(moveDirection == eMoveDirection.Right)
		{
			xSpeed = xSpeed - resistance * TimerManager.Instance.GetDeltaTime;
			xSpeed = xSpeed < 0 ? 0 : xSpeed;
		}
		else if(moveDirection == eMoveDirection.Left)
		{
			xSpeed = -xSpeed + resistance * TimerManager.Instance.GetDeltaTime;
			xSpeed = xSpeed > 0 ? 0 :xSpeed;
		}
		else 
		{
			xSpeed = 0;
			Debug.Log(" ====================== Speed is not 0 , But moveDirection is None! ");
		}
		
		return xSpeed;
	}

	protected const float ImpactYLostRatio = 0.5f;
	protected const float ImpactXLoatRatio = 0.2f;

	public static Vector3 GetImpactSpeedBy(Vector3 enterSpeed , eImpactMothod mothod)
	{
		Vector3 impact = Vector3.zero;
		float xSpeed = Mathf.Abs(enterSpeed.x) * ImpactXLoatRatio;
		float ySpeed = Mathf.Abs(enterSpeed.y) * ImpactYLostRatio;
	
		if(mothod == eImpactMothod.Ground)
		{
			impact.x = enterSpeed.x > 0 ? xSpeed : -xSpeed;
			impact.y = enterSpeed.y < 0 ? ySpeed : -ySpeed;

		}
		else
		{
			Debug.LogError(" ==============================  Not Yet!!");
		}

		return impact;
	}
}
