using UnityEngine;
using System.Collections;

public class CameraUIS : MonoBehaviour 
{
	public CameraDef type = CameraDef.Default;
	
	protected Camera m_camera = null;

	protected GameObject followTarget = null;

	protected CameraFollowMethod followMethod = null;

	protected bool m_isFollowing = false;
		
	protected bool m_isActive = false;
	public bool IsActive
	{
		get{return m_isActive;}
	}
	
	void Awake () 
	{
		if(type != CameraDef.Default)
		{
			CameraManager.Instance.addCamera(this);
		}
		
		m_camera = this.GetComponent<Camera>();
	}

	void LateUpdate()
	{
		if(m_isFollowing)
		{
			if(followMethod != null)
			{
				Vector3 nextPos = followMethod.UpdateFollowPos();
				this.transform.position = nextPos;
			}
		}
	}

	public void StartFollow(GameObject target , CameraFollowMode mode)
	{
		if(target != null)
		{
			m_isFollowing = true;
			followTarget = target;

			if(followMethod != null)
			{
				followMethod.Destroy();
			}

			if(mode == CameraFollowMode.Limit)
			{
				followMethod = new CameraLimitFollow();
			}
			else if(mode == CameraFollowMode.Free)
			{
				followMethod = new CameraFreeFollow();
			}

			followMethod.Initalze(target, m_camera);
		}
	}

	public void StopFollow()
	{
		m_isFollowing = false;
	}

	public void SetCameraEnalbe(bool value)
	{
		if(m_isActive != value &&  this.camera != null)
		{
			m_isActive = value;
			m_camera.enabled = m_isActive;

			if(!m_isActive)
			{
				m_isFollowing = false;
			}
		}
	}

	public Camera getCamera()
	{
		return m_camera;
	}
}

public class CameraFollowMethod
{
	protected GameObject m_target = null;

	protected Camera m_camera = null;

	virtual	public void Initalze(GameObject target , Camera camera)
	{
		m_target = target;
		m_camera = camera;
	}

	virtual public void Destroy()
	{
		m_target = null;
		m_camera = null;
	}

	virtual public Vector3 UpdateFollowPos()
	{
		return Vector3.zero;
	}
}

public class CameraFreeFollow : CameraFollowMethod
{
	public override void Initalze (GameObject target, Camera camera)
	{
		base.Initalze (target , camera);
	}

	public override Vector3 UpdateFollowPos ()
	{
		Vector3 follow = m_target.transform.position;
		Vector3 pos = m_camera.transform.position;
		
		pos.x = follow.x;
		pos.y = follow.y;

		return pos;
	}
}

public class CameraLimitFollow : CameraFollowMethod
{
	protected float m_xMin = 0f;

	protected float m_xMax = 0f;

	public override void Initalze (GameObject target, Camera camera)
	{
		base.Initalze (target , camera);

		SetLimit();
	}
	
	public override Vector3 UpdateFollowPos ()
	{
		Vector3 follow = m_target.transform.position;
		Vector3 pos = m_camera.transform.position;

		if(m_xMin < m_xMax)
		{
			if(follow.x <= m_xMin)
			{
				pos.x = m_xMin;
			}
			else if(follow.x >= m_xMax)
			{
				pos.x = m_xMax;
			}
			else
			{
				pos.x = follow.x;
			}
		}

		return pos;
	}

	protected void SetLimit()
	{
		float dis = m_target.transform.position.z - m_camera.transform.position.z;
		
		Vector2 vect = GetPerspectiveView(dis,m_camera);
		//Vector3 leftPoint = m_camera.transform.position;
		m_xMin = RoomColliderManager.GetXMin + vect.x ;
		//Vector3 RightPoint =  m_camera.transform.position;
		m_xMax = RoomColliderManager.GetXMax - vect.x;

		//Debug.Log(m_xMin + ","+ m_xMax + "," + RoomColliderManager.GetXMin +","+ RoomColliderManager.GetXMax +","+vect+"==============");
	}

	protected Vector2 GetPerspectiveView(float distance , Camera camera)
	{
		//Transform tx = camera.transform;
		float halfFOV = ( camera.fieldOfView * 0.5f ) * Mathf.Deg2Rad;
		float aspect = camera.aspect;
		
		float height = distance * Mathf.Tan( halfFOV );
		float width = height * aspect;
		
		return new Vector2(width,height);
	}
}
