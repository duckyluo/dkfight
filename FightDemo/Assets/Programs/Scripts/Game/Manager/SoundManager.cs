using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SoundManager
{
	protected static Dictionary<SoundDef , UisSoundObj> m_soundDict = new Dictionary<SoundDef, UisSoundObj>();
	
	public static void Initalize()
	{
		m_soundDict.Clear();
	}

	public static bool AddSound(SoundDef name , UisSoundObj soundObj)
	{
		if(!m_soundDict.ContainsKey(name))
		{
			m_soundDict.Add(name,soundObj);
			return true;
		}
		else
		{
			Debug.LogError("Sound name is repeat");
			return false;
		}
	}

	public static UisSoundObj GetSound(SoundDef name)
	{
		UisSoundObj sound = null;
		if(m_soundDict.TryGetValue(name , out sound))
		{
			return sound;
		}
		else return null;
	}

	public static bool PlaySound(SoundDef name)
	{
		UisSoundObj sound = GetSound(name);
		if(sound != null)
		{
			sound.PlaySound();
			return true;
		}
		else return false;
	}
}
