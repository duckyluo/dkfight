using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dk.Util
{
	public class DkLog : MonoBehaviour 
	{
		private static List<string> mLogLines = new List<string>();

		private static List<string> mErrorLines = new List<string>();

		private static List<string> mWriteTxt = new List<string>();
		
		private string outpath;

		private static bool inited = false;

		public static bool GuiErrorEnalbe = true;

		public static int LogCacheNum = 20;

		#region singleton
		private static DkLog s_instance = null;
		
		public static DkLog Instance
		{
			get
			{
				if(!s_instance)
				{
					s_instance = GameObject.FindObjectOfType(typeof(DkLog)) as DkLog;
					if (!s_instance)
					{
						GameObject container = new GameObject();
						container.name = "DkLog";
						s_instance = container.AddComponent(typeof(DkLog)) as DkLog;
					}
				}

				return s_instance;
			}
		}
		#endregion

		public void OnGUI()
		{
			if(GuiErrorEnalbe)
			{
				GUI.color = Color.red;
				for (int i = 0, imax = mErrorLines.Count; i < imax; ++i)
				{
					GUILayout.Label(mErrorLines[i]);
				}
			}
		}

		public void Awake()
		{
			DontDestroyOnLoad(this);
		}

		public void Initalize()
		{
			if(inited == false)
			{
				inited = true;

				outpath = Application.persistentDataPath + "/DkLog.txt";
				
				if (System.IO.File.Exists (outpath)) 
				{
					File.Delete (outpath);
				}
				
				Application.RegisterLogCallback(HandleLog);
			}
		}

		public void Update () 
		{
			if(mWriteTxt.Count > 0)
			{
				string[] temp = mWriteTxt.ToArray();
				foreach(string t in temp)
				{
					using(StreamWriter writer = new StreamWriter(outpath, true, Encoding.UTF8))
					{
						writer.WriteLine(t);
					}
					mWriteTxt.Remove(t);
				}
			}
		}

		protected static void HandleLog(string logString, string stackTrace, LogType type)
		{
			if(LogType.Warning != type)
			{
				AddLogLine(logString);
			}

			if (type == LogType.Error || type == LogType.Exception) 
			{
				AddErrorLog(logString);
				AddErrorLog(stackTrace);
			}
		}

		protected static void AddLogLine(params object[] objs)
		{
			string text = "";
			for (int i = 0; i < objs.Length; ++i)
			{
				if(objs[i] != null)
				{
					if (i == 0)
					{
						text += objs[i].ToString();
					}
					else
					{
						text += ", " + objs[i].ToString();
					}
				}
			}
			if (Application.isPlaying)
			{
				if (mLogLines.Count > LogCacheNum) 
				{
					mLogLines.RemoveAt(0);
				}
				mLogLines.Add(text+"\n");
				mWriteTxt.Add(text);
			}
		}

		protected static void AddErrorLog(params object[] objs)
		{
			string text = "";
			for (int i = 0; i < objs.Length; ++i)
			{
				if(objs[i] != null)
				{
					if (i == 0)
					{
						text += objs[i].ToString();
					}
					else
					{
						text += ", " + objs[i].ToString();
					}
				}
			}
			if (Application.isPlaying)
			{
				if (mErrorLines.Count > LogCacheNum) 
				{
					mErrorLines.RemoveAt(0);
				}
				mErrorLines.Add(text);
			}
		}

		public static List<string> GetLogList()
		{
			return mLogLines;
		}

		public static void Show(object message)
		{
			LogType type = LogType.Log;
			Show(message,type);
		}
		
		public static void Show(object message , LogType type)
		{
			if(DkGlobal.GetDebugEnable())
			{
				switch(type)
				{
				case LogType.Log:
					Debug.Log(message);
					break;
				case LogType.Warning:
					Debug.LogWarning(message);
					break;
				case LogType.Error:
					Debug.LogError(message);
					break;
				default:
					Debug.Log(message);
					break;
				}
			}
			else
			{
				if(type == LogType.Error)
				{
					Debug.LogError(message);
				}
				else
				{
					HandleLog(message.ToString(),null,LogType.Log);
				}
			}
		}

	}
}
