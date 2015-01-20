using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Util;
using Dk.Loader;
using UnityEngine;

namespace Dk.Asset
{
	public enum eDkAssetPtType
	{
		ASSET_PT_PC = 0,
		ASSET_PT_AND = 1,
		ASSET_PT_IOS = 2,
	}

	public class DkABItem : IComparable
	{
		public string path = string.Empty;
		public eDkAssetPtType ptType = eDkAssetPtType.ASSET_PT_PC;
		public string depends = string.Empty;
		
		public string GetLoadPath()
		{
			return DkLoadPathUtil.GetABPath(this.path,this.ptType);
		}

		public string GetABName
		{
			get
			{
				if(!string.IsNullOrEmpty(path))
				{
					return DkFileUtil.GetFileNameByFull(path,false);
				}
				else return "";
			}
		}

		public string GetMainAssetGuid
		{
			get
			{
				return GetABName;
			}
		}

		public List<string> GetDependList()
		{
			List<string> list = new List<string>();
			if(depends.Length > 0)
			{
				string[] strs = depends.Split(new char[]{'|'});
				foreach(string s in strs)
				{
					if(!string.IsNullOrEmpty(s) && s.Length > 0)
					{
						list.Add(s);
					}
				}
			}
			return list;
		}

		public bool isDepend(DkABItem item)
		{
			if(GetDependList().Contains(item.GetMainAssetGuid))
			{
				return true;
			}
			else return false;
		}

		public int CompareTo(object obj)
		{
			int res = 0;
			try
			{
				DkABItem item = (DkABItem)obj;
				if(this.GetDependList().Count > item.GetDependList().Count)
				{
					res = 1;
				}
				else if(this.GetDependList().Count < item.GetDependList().Count)
				{
					res = -1;
				}
			} 
			catch(Exception ex)
			{
				Debug.Log(ex.StackTrace);
			}
			return res;
		}

		public override string ToString ()
		{
			string strPath = "\"\"";
			if(!string.IsNullOrEmpty(path))
			{
				strPath = path;
			}
			
			string strDepends = "\"\"";
			if(!string.IsNullOrEmpty(depends))
			{
				strDepends = depends;
			}

			return strPath+","+(int)ptType+","+strDepends;
		}

		public bool Parse(string line)
		{
			string[] list = line.Split(new char[]{','});
			if(list.Length == 3)
			{
				this.path = list[0];
				int stype = 0;
				System.Int32.TryParse(list[1],out stype);
				this.ptType = (eDkAssetPtType)stype;
				this.depends = list[2];
				return true;
			}
			else return false;
		}

		public bool Parse(List<string> list)
		{
			if(list.Count == 3)
			{
				this.path = list[0];
				int stype = 0;
				System.Int32.TryParse(list[1],out stype);
				this.ptType = (eDkAssetPtType)stype;
				this.depends = list[2];
				return true;
			}
			else return false;
		}
	}
}
