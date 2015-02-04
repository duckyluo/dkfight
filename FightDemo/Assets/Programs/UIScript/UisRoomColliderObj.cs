using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UisRoomColliderObj : MonoBehaviour 
{
	public BoxCollider floorBox;
	public BoxCollider leftBox;
	public BoxCollider rightBox;

	protected float m_xMin = 0f;
	public float XMin
	{
		get{return m_xMin;}
	}

	protected float m_xMax = 0f;
	public float XMax
	{
		get{return m_xMax;}
	}

	protected float m_ymin = 0f;
	public float YMin
	{
		get{return m_ymin;}
	}
	
	void Awake () 
	{
		if(floorBox == null || leftBox == null || rightBox == null)
		{
			Debug.LogError("RoomUIS Not Set");
			return;
		}
	
		m_ymin = floorBox.transform.localPosition.y + floorBox.size.y/2+0.1f;
		m_xMin = leftBox.transform.localPosition.x + leftBox.size.x/2+0.1f;
		m_xMax = rightBox.transform.localPosition.x - rightBox.size.x/2-0.1f;

		RoomColliderManager.CurRoom = this;
	}
}

public class RoomColliderManager
{
	protected static UisRoomColliderObj m_room = null;
	public static UisRoomColliderObj CurRoom
	{
		set{m_room = value;}
	}

	public static float GetXMin
	{
		get
		{
			if(m_room != null)
			{
				return m_room.XMin;
			}
			else return 0f;
		}
	}
	
	public static float GetXMax
	{
		get
		{
			if(m_room != null)
			{
				return m_room.XMax;
			}
			else return 0f;
		}
	}
	
	public static float GetYMin
	{
		get
		{
			if(m_room != null)
			{
				return m_room.YMin;
			}
			else return 0f;
		}
	}
}

