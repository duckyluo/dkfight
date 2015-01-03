using System;
using System.Collections;
using System.Collections.Generic;

public class SceneManager
{
	private static SceneObjCtrl m_objCtrl;

	private static bool m_isInited = false;

	public static void Initalize()
	{
		m_objCtrl = new SceneObjCtrl();
		m_isInited = true;
	}

	public static SceneObj AddNewItem(SceneObjInfo info)
	{
		if(!m_isInited)
		{
			return null;
		}

		return m_objCtrl.AddNewItem(info);
	}

	public static void ClearAll()
	{
		if(!m_isInited)
		{
			return;
		}

		m_objCtrl.ClearAllObj();
	}

	public static void Update()
	{
		if(!m_isInited)
		{
			return;
		}

		m_objCtrl.Update();
	}
}
