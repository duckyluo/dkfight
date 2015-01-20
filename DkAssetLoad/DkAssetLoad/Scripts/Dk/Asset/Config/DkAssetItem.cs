using System.Collections;
using System.Collections.Generic;
using Dk.Util;
using UnityEngine;

namespace Dk.Asset
{
	public enum eDkAssetSourceType
	{
		SRC_TYPE_OTHERS = 0,
		SRC_TYPE_GAMEOBJECT = 1,
		SRC_TYPE_SCENE = 2,
		SRC_TYPE_SOUND = 3,
	}

	public enum eDKAssetLoadType
	{
		FROM_SOURCE = 0,
		FORM_ASSETBUNDLE = 1,
	}

	public class DkAssetItem
	{
		public string guid = string.Empty;
		public string path = string.Empty;
		public int assetType = 0;
		public string inABName = string.Empty;
		
		public System.Type GetAssetType
		{
			get
			{
				if(this.assetType == (int)eDkAssetSourceType.SRC_TYPE_GAMEOBJECT)
				{
					return typeof(UnityEngine.GameObject);
				}
				else 
				{
					return typeof(UnityEngine.GameObject);
				}
			}
		}
	
		public string AssetPath
		{
			get
			{
				return "Assets/"+path;
			}
		}

		public string AssetName
		{
			get
			{
				return DkFileUtil.GetFileNameByFull(path,false);
			}
		}
						
		public override string ToString ()
		{
			string strGuid = "\"\"";
			if(!string.IsNullOrEmpty(guid))
			{
				strGuid = guid;
			}

			string strPath = "\"\"";
			if(!string.IsNullOrEmpty(path))
			{
				strPath = path;
			}

			string strInABName = "\"\"";
			if(!string.IsNullOrEmpty(inABName))
			{
				strInABName = inABName;
			}

			return strGuid+","+strPath+","+assetType+","+strInABName;
		}
				
		public void Destroy()
		{
			this.guid = null;
			this.path = null;
			this.inABName = null;
		}

		public DkAssetItem Clone()
		{
			DkAssetItem item = new DkAssetItem();
			item.guid = this.guid;
			item.path = this.path;
			item.assetType = this.assetType;
			item.inABName = this.inABName;
			return item;
		}

		public bool Parse(string line)
		{
			string[] list = line.Split(new char[]{','});
			if(list.Length == 4)
			{
				this.guid = list[0];
				this.path = list[1];
				this.assetType = System.Int32.Parse(list[2]);
				this.inABName = list[3];
				return true;
			}
			else return false;
		}

		public bool Parse(List<string> list)
		{
			if(list.Count == 4)
			{
				this.guid = list[0];
				this.path = list[1];
				this.assetType = System.Int32.Parse(list[2]);
				this.inABName = list[3];
				return true;
			}
			else return false;
		}
	}
}