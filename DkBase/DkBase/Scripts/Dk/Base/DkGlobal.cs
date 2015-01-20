using UnityEngine;
using Dk.Util;
using System.IO;

public class DkGlobal
{
	public static string CacheDir = "CacheData/";
	public static string FileName = "DkGlobal.bytes";

	private static string version = "0.0.0";
	private static int netEnable = 0;
	private static int testMode = 1;
	private static int sourceDataEnable = 0;
	private static int debugEnable = 1;

	public static bool Initalize()
	{
		if(DkFileUtil.ExistFile(GetLoadPath()))
		{
			string content = DkFileUtil.ReadFileToString(GetLoadPath());
			return Parse(content);
		}
		else return false;
	}

	public static void CacheClean()
	{
		string dir = Application.persistentDataPath+"/"+CacheDir;
		DkFileUtil.DeleteDir(dir);

		DkLog.Show("[Dk] Cache Clean");
	}

	public static void SaveData()
	{
		string url = Application.persistentDataPath+"/"+CacheDir;
		if(Directory.Exists(url) == false)
		{
			Directory.CreateDirectory(url);
		}
		url += FileName;
		string content = ToOutputString();
		DkFileUtil.WriteFile(url,content,false);

		DkLog.Show("[Dk] Global Save");
	}

	public static string GetLoadPath()
	{
		return Application.persistentDataPath+"/"+CacheDir+FileName;
	}

	public static string ToOutputString()
	{
		string str = version+"|"+netEnable+"|"+testMode+"|"+sourceDataEnable;
		return str;
	}

	public static bool Parse(string str)
	{
		string[] list = str.Split(new char[]{'|'});
		if(list.Length >= 4)
		{
			version = list[0];
			System.Int32.TryParse(list[1],out netEnable);
			System.Int32.TryParse(list[2],out testMode);
			System.Int32.TryParse(list[3],out sourceDataEnable);
			//System.Int32.TryParse(list[4],out debugEnable);
			DkLog.Show("[Dk] Global Load");
			return true;
		}
		else
		{
			DkLog.Show("[error] Global Parse error",LogType.Error);
			return false;
		}
	}

	public static void SetVersion(string value)
	{
		version = value;
	}

	public static string GetVersion()
	{
		return version;
	}

	public static void SetNetEnable(bool value)
	{
		if(value)
		{
			netEnable = 1;
		}
		else netEnable = 0;

	}
	
	public static bool GetNetEnable()
	{
		return (netEnable != 0);
	}

	public static void SetTestMode(bool value)
	{
		if(value)
		{
			testMode = 1;
		}
		else testMode = 0;
	}
	
	public static bool GetTestMode()
	{
		return (testMode != 0);
	}

	public static void SetSourceDataEnable(bool value)
	{
		if(value)
		{
			sourceDataEnable = 1;
		}
		else sourceDataEnable = 0;
	}
	
	public static bool GetSourceDataEnable()
	{
		if(Application.platform == RuntimePlatform.WindowsEditor)
		{
			return (sourceDataEnable != 0);
		}
		else
		{
			return false;
		}
	}

	public static void SetDebugEnable(bool value)
	{
		if(value)
		{
			debugEnable = 1;
		}
		else 
		{
			debugEnable = 0;
		}
	}
	
	public static bool GetDebugEnable()
	{
		return (debugEnable != 0);
	}
}