using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Dk.Util
{
	public enum eFilePathType
	{
		DATA_PATH = 0,
		PERSISTENT_DATA_PATH = 1,
		NEW_WORK = 2,
	}

	public class DkFileUtil : MonoBehaviour
	{
		public static List<FileInfo> GetAllFilesInPath(string strPath)
		{
			List<FileInfo> fileList = new List<FileInfo>();
			
			if (Directory.Exists(strPath) == false)
			{
				DkLog.Show("[error]Directory Not Found! : " + strPath,LogType.Error);
				return fileList;
			}
			DirectoryInfo di = new DirectoryInfo(strPath);
			DirectoryInfo[] aDirInfo = di.GetDirectories();
			
			if (aDirInfo != null)
			{
				foreach (DirectoryInfo Data in aDirInfo)
				{
					addDirFile(Data,fileList);
				}
			}
			
			AddCurFileInFolder(di,fileList);
			return fileList;
		}
		
		private static void addDirFile(DirectoryInfo DirInfo, List<FileInfo> list)
		{
			string strPath = DirInfo.FullName;
			DirectoryInfo di = new DirectoryInfo(strPath);
			DirectoryInfo[] aDirInfo = di.GetDirectories();
			
			if (aDirInfo != null)
			{
				foreach (DirectoryInfo Data in aDirInfo)
				{
					addDirFile(Data,list);
				}
			}
			
			AddCurFileInFolder(di,list);
		}
		
		private static void AddCurFileInFolder(DirectoryInfo di,List<FileInfo> list)
		{
			FileInfo[] fis = di.GetFiles();
			
			if (fis == null)
				return;
			
			foreach (FileInfo file in fis)
			{
				if (file.Extension.Contains("meta") == false)
				{
					list.Add(file);
				}
			}
		}

		public static string GetFilePathByRelative(string url)
		{
			string _path = "file://"+Application.dataPath+"/"+url;
			return _path;
		}

		public static string GetRelativePathByFull(string FullName,bool haveAsset)
		{
			string Name = FullName;
			string AssetName = "Assets/";
			int EndPos = Name.LastIndexOf(AssetName);
			
			if (EndPos < 0)
				return "";
			if(haveAsset == false)
			{
				EndPos += AssetName.Length;
			}
			
			char[] ExN = Name.ToCharArray(0, Name.Length);
			
			string ExtName = string.Empty;
			
			for (int ik = EndPos; ik < ExN.Length; ik++)
			{
				ExtName += ExN[ik].ToString();
			}
			
			return ExtName;
		}

		public static string GetLastNameByFull(string FullName)
		{
			string[] strs = FullName.Split(new char[]{'.'});
			string lastName = string.Empty;
			if(strs.Length >1)
			{
				lastName = strs[strs.Length -1];
			}
			return lastName;
		}
		
		public static string GetDirPathByFull(string FullName)
		{
			string[] strs = FullName.Split(new char[]{'/'});
			string dirPath = string.Empty;
			if(strs.Length >1)
			{
				for(int i = 0; i < strs.Length-1;i++)
				{
					dirPath += strs[i]+"/";
				}
			}
			
			return dirPath;
		}

		public static string GetFileNameByFull(string FullName)
		{
			return DkFileUtil.GetFileNameByFull(FullName,true);
		}

		public static string GetFileNameByFull(string FullName,bool haveLastName)
		{
			int EndPos = FullName.LastIndexOf("/") + 1;
			char[] ExN = FullName.ToCharArray(0, FullName.Length);
			string ExtName = string.Empty;
			
			if (EndPos < ExN.Length) 
			{
				for (int ik = EndPos; ik < ExN.Length; ik++) 
				{
					ExtName += ExN[ik].ToString();
				}
			}
			if(haveLastName == false)
			{
				string[] strs = ExtName.Split(new char[]{'.'});
				if(strs.Length >1)
				{
					ExtName = string.Empty;
					for(var i= 0 ; i < strs.Length-1;i++)
					{
						if(i < strs.Length - 2)
						{
							ExtName += strs[i]+".";
						}
						else
						{
							ExtName += strs[i];
						}
					}
				}
				else
				{
					Debug.Log("[error][GetFileNameByFull] there is something wrong ,path "+FullName);
				}
			}
			
			return ExtName;
		}

		public static string MakeKeyCodeName(string AssetBundleName, int Length, int Version)
		{
			string EncodeName = string.Empty;
			EncodeName = AssetBundleName + "_" + Length.ToString() + "_" + Version.ToString();
			string strMD5Code = EncodingMD5(EncodeName);
			
			//Util.DebugLog("Start Encode : " + EncodeName +"---->"+strMD5Code);
			return strMD5Code;
		}
		
		public static string EncodingMD5(string input)
		{
			System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
			
			byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
			byte[] hash = sha1.ComputeHash(inputBytes);

			string strData = string.Empty;
			int Count = hash.GetLength(0);
			for (int i = 0; i < Count; i++)
			{
				int Number = (int)hash[i];
				
				strData += Number.ToString("X2");
				
			}
			
			return strData;
		}
		
		private static string neturl = "";
		public static void SetAssetNetPath(string url)
		{
			neturl = url;
		}
				
		public static string GetFileDataPath(eFilePathType type)
		{
			string filepath = "";
			if(type == eFilePathType.DATA_PATH)
			{
				if(Application.platform == RuntimePlatform.IPhonePlayer)
				{
					filepath = Application.dataPath + "/Raw/";
				}
				else if(Application.platform == RuntimePlatform.Android)
				{
					filepath = "jar:file://"+Application.dataPath + "!/assets/";
				}
				else if(Application.platform == RuntimePlatform.WindowsEditor)
				{
					filepath = Application.dataPath+"/";
				}
				else if(Application.platform == RuntimePlatform.WindowsPlayer)
				{
					filepath = Application.dataPath+ "/StreamingAssets/";
				}
				else
				{
					filepath = Application.dataPath + "/StreamingAssets/";
				}
			}
			else if(type == eFilePathType.PERSISTENT_DATA_PATH)
			{
				filepath = Application.persistentDataPath +"/";
			}
			else if(type == eFilePathType.NEW_WORK)
			{
				filepath = neturl + "/";
			}

			return filepath;
		}

		public static void WriteFile(string path , string content)
		{
			WriteFile(path,content,false);
		}

		public static void WriteFile(string path , string content , bool append)
		{
			StreamWriter sw;
			FileInfo file = new FileInfo(path);
			if(file.Exists && append == true)
			{
				sw = file.AppendText();
			}
			else 
			{
				sw = file.CreateText();
			}

			sw.Write(content);
			sw.Flush();
			sw.Close();
			sw.Dispose();
		}
		
		public static string ReadFileToString(string path)
		{
			string content = "";
			using(StreamReader sr =  File.OpenText(path))
			{
				string line;
				while((line = sr.ReadLine()) != null)
				{
					content += line + "\n";
				}
			}
//			StreamReader sr = null;
//			try
//			{
//				sr = File.OpenText(path);
//			}
//			catch(Exception ex)
//			{
//				Debug.Log("[error] : "+ex.StackTrace);
//				return null;
//			}

//			string content = "";
//			string line;
//			while((line = sr.ReadLine()) != null)
//			{
//				content += line + "\n";
//			}
//
//			sr.Close();
//			sr.Dispose();

			return content;
		}

		public static void WriteFile(string path , byte[] bytes , bool zip)
		{
			FileStream fileStream = File.Create(path);
			byte[] outputbytes = bytes ;
			if(zip)
			{
				outputbytes = DkZipUtil.GZip(bytes);
			}

			if(outputbytes != null && outputbytes.Length > 0)
			{
				fileStream.Write(outputbytes,0,outputbytes.Length);
				fileStream.Flush();
			}
			fileStream.Close();
			fileStream.Dispose();
		}

		public static byte[] ReadFileToBytes(string path , bool unzip)
		{
			if(!File.Exists(path))
			{
				return null;
			}

			byte[] inputbytes = null;
			using(FileStream filestream = File.Open(path,FileMode.Open))
			{
				int len = (int)filestream.Length;
				byte[] bytes = new byte[len]; 
				filestream.Read(bytes,0,bytes.Length);
				inputbytes = bytes;
				if(unzip)
				{
					inputbytes = DkZipUtil.unGZip(bytes);
				}
			}
	
//			filestream.Close();
//			filestream.Dispose();
			return inputbytes;
		}

		public static void DeleteDir(string dir)
		{
			if(Directory.Exists(dir))
			{
				Directory.Delete(dir,true);
			}
		}

		public static void DeleteFile(string path)
		{
			if(File.Exists(path))
			{
				File.Delete(path);
			}
		}

		public static bool ExistFile(string path)
		{
			return File.Exists(path);
		}
	}
}
