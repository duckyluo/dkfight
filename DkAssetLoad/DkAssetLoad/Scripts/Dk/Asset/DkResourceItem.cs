using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Event;
using Dk.Loader;


namespace Dk.Asset
{
	public class DkResourceLoadItem
	{
		public string loadPath = string.Empty;

		public eDkWWWItemType type = eDkWWWItemType.ASEET_BUNDLE;

		public object obj = null;

		public int referCount = 0;
		
		public void Destroy()
		{
			this.loadPath = null;
			this.obj = null;
		}

		public override string ToString ()
		{
			return loadPath + ","+type.ToString() + ","+obj.ToString();
		}
	}

	public class DkResourceAssetItem
	{
		public string itemPath = string.Empty;

		public eDkAssetSourceType assetType = eDkAssetSourceType.SRC_TYPE_OTHERS;

		public UnityEngine.Object obj = null;

		public DateTime updateTime;

		public System.Type GetAssetType
		{
			get
			{
				if(this.assetType == eDkAssetSourceType.SRC_TYPE_GAMEOBJECT)
				{
					return typeof(UnityEngine.GameObject);
				}
				else 
				{
					return typeof(UnityEngine.GameObject);
				}
			}
		}

		public void Destroy()
		{
			this.itemPath = null;
			this.obj = null;
		}

		public override string ToString ()
		{
			return itemPath + ","+assetType.ToString() + ","+obj.ToString();
		}
	}
}
