using UnityEngine;
using System.Collections;

public class RoleRootPoint : MonoBehaviour 
{
	void Awake()
	{
		this.enabled = false;
	}

	public Vector3 GetLocalPos
	{
		get
		{
			return this.transform.localPosition;
		}
	}
	
	protected Vector3 m_oldPos = Vector3.zero;
	protected bool m_isBegin = false;
	public Vector3 UpdateRecord()
	{
		if(m_isBegin == false)
		{
			m_isBegin = true;
			m_oldPos = this.transform.localPosition;
			return Vector3.zero;
		}
		else
		{
			Vector3 record = this.transform.localPosition - m_oldPos;
			m_oldPos = this.transform.localPosition;
			return record;
		}
	}
	
	public void ResetRecord()
	{
		m_isBegin = false;
		m_oldPos = Vector3.zero;
	}
	
}
