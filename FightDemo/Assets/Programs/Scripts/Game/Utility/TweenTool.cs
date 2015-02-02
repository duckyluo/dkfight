using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TweenTool : MonoBehaviour
{
	public const string SHAKE = "shake";

	public const string ROTATION = "rotation";
	
	protected GameObject m_target = null;

	protected TweenInfo m_shakeInfo = new TweenInfo();

	protected TweenInfo m_rotateInfo = new TweenInfo();
	
	public void Start()
	{
		m_target = this.gameObject;
	}

	public void Destroy()
	{
		StopAll();
		m_shakeInfo = null;
		m_target = null;
	}

	public void StopAll()
	{
		StopShake();
		StopRotation();
	}

	//========================================================================//

	public void Shake()
	{
		Vector3 motion = new Vector3(0,0,0.07f);
		float time = 0.2f;
		Vector3 resetPos = Vector3.zero;
		Shake(motion,time,resetPos,true);
	}

	public void Shake(Vector3 motion , float time , Vector3 resetPos , bool endReset)
	{
		if(m_shakeInfo.GetStatus == eProcessStatus.Run)
		{
			StopShake();
		}

		m_shakeInfo.GetStatus = eProcessStatus.Run;
		m_shakeInfo.changeValue = motion;
		m_shakeInfo.time = time;
		m_shakeInfo.resetValue = resetPos;
		m_shakeInfo.endReset = endReset;

		iTween.ShakePosition(m_target, iTween.Hash("name",SHAKE,
													"amount", m_shakeInfo.changeValue,
		                                           "time", m_shakeInfo.time,
		                                           "islocal",true,
		                                           "oncomplete", "OnCompleteShake" ));


	}

	public void StopShake()
	{
		m_shakeInfo.GetStatus = eProcessStatus.End;
		iTween.StopByName(m_target,SHAKE);
		if(m_shakeInfo.endReset)
		{
			m_target.transform.localPosition = m_shakeInfo.resetValue;
		}
	}

	public bool IsShaking
	{
		get{return (m_shakeInfo.GetStatus == eProcessStatus.Run);}
	}
	
	private void OnCompleteShake()
	{
		//Debug.Log(" ================================ OnCompleteShake");
		m_shakeInfo.GetStatus = eProcessStatus.End;
		if(m_shakeInfo.endReset)
		{
			m_target.transform.localPosition = m_shakeInfo.resetValue;
		}
	}


	//========================================================================//


	public void RotateTo(Vector3 ratation , float time , Vector3 resetRatation , bool endReset)
	{
		if(m_rotateInfo.GetStatus == eProcessStatus.Run)
		{
			StopRotation();
		}
//		
		m_rotateInfo.GetStatus = eProcessStatus.Run;
		m_rotateInfo.changeValue = ratation;
		m_rotateInfo.time = time;
		m_rotateInfo.resetValue = resetRatation;
		m_rotateInfo.endReset = endReset;

		iTween.RotateTo(m_target, iTween.Hash("name",ROTATION,
		                                      "rotation", m_rotateInfo.changeValue,
		                                      "time", m_rotateInfo.time,
		                                      	"oncomplete", "OnCompleteRotate" ));

	}

	public void StopRotation()
	{
		m_rotateInfo.GetStatus = eProcessStatus.End;
		iTween.StopByName(m_target,ROTATION);
		if(m_rotateInfo.endReset)
		{
			Quaternion rotation = Quaternion.identity;
			rotation.eulerAngles = m_rotateInfo.resetValue;
			m_target.transform.localRotation = rotation;
		}
	}

	public bool IsRotating
	{
		get{return (m_rotateInfo.GetStatus == eProcessStatus.Run);}
	}

	
	private void OnCompleteRotate()
	{
		m_rotateInfo.GetStatus = eProcessStatus.End;
		//Debug.Log(" ================================ OnCompleteRotate");
		if(m_rotateInfo.endReset)
		{
			Quaternion rotation = Quaternion.identity;
			rotation.eulerAngles = m_rotateInfo.resetValue;
			m_target.transform.localRotation = rotation;
		}
	}
}

public class TweenInfo
{
	public Vector3 changeValue = Vector3.zero;
	public Vector3 resetValue = Vector3.zero;
	public float time = 0f;
	public bool endReset = true;

	protected eProcessStatus m_status = eProcessStatus.None;
	public eProcessStatus GetStatus
	{
		set{m_status = value;}
		get{return m_status;}
	}
}