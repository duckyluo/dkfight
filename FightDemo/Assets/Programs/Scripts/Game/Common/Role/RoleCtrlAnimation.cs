using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Util;

public class RoleCtrlAnimation
{
	//protected string m_curAniName = string.Empty;

	protected Animation m_animation = null;

	protected AnimationCtrl aniCtrl = null;

	protected bool m_isInited = false;

	protected RoleBlackBoard m_bbData;

	public void Initalize(RoleBlackBoard bbData)
	{
		m_bbData = bbData;

		if(GetAnimation == null)
		{
			Debug.LogError("[error ]Some thing is null : " +" ani : "+this.m_animation + " ");
			m_isInited = false;
		}
		else 
		{
			m_isInited = true;
			aniCtrl = new AnimationCtrl();
			aniCtrl.Initalize(m_animation);
		}
	}

//	public void Update()
//	{
//		if (GetRunTimeData.AnimationName != aniCtrl.GetCurAniName) 
//		{
//			Play(
//		}
//	}
	
	public void Play(string name , WrapMode mode , bool replay)
	{
		if (replay || aniCtrl.GetCurAniName != name) 
		{
			aniCtrl.Play (name, mode);
		}
	}

	public void SetCurSpeed(float value)
	{
		aniCtrl.SetCurSpeed (value);
	}
	
	public void Pause()
	{
		aniCtrl.Pause();
	}

	public void Resume()
	{
		aniCtrl.Resume ();
	}

	public AnimationState GetCurState
	{
		get
		{
			return aniCtrl.GetCurState;
		}

	}

	protected Animation GetAnimation
	{
		get
		{
			if(m_animation == null && m_bbData != null)
			{
				m_animation = (m_bbData as RoleBlackBoard).PrefabModel.GetComponent<Animation> ();
			}
			return m_animation;
		}
	}

	protected RoleDataRunTime GetRunTimeData
	{
		get
		{
			if(m_bbData != null)
			{
				return m_bbData.DataRunTime;
			}
			else return null;
		}
	}
	



}
