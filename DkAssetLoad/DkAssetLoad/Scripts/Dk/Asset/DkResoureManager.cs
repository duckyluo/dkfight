using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dk.Util;
using Dk.Loader;
using Dk.Event;

namespace Dk.Asset
{
	public class DkResourceManager  
	{
		private static Dictionary<string,DkResourceLoadItem> loadMap = new Dictionary<string,DkResourceLoadItem>();

		private static Dictionary<string,DkResourceAssetItem> assetMap = new Dictionary<string, DkResourceAssetItem>();

		private static Dictionary<string,DkResourceAssetItem> recyclePool = new Dictionary<string, DkResourceAssetItem>();

		private static DkResourceManager s_instance = null;
		
		public static DkResourceManager Instance
		{
			get
			{
				if (null == s_instance)
				{
					s_instance = new DkResourceManager();
				}
				
				return s_instance;
			}
		}
	
		public void AddLoadItem(DkResourceLoadItem item)
		{
			if(!ContainsLoadItem(item.loadPath))
			{
				loadMap.Add(item.loadPath,item);
			}
		}

		public void RemoveLoadItem(string loadPath)
		{
			DkResourceLoadItem loadItem = GetLoadItem(loadPath);
			if(loadItem != null)
			{
				loadMap.Remove(loadPath);
				loadItem.Destroy();
			}
		}

		public DkResourceLoadItem GetLoadItem(string loadPath)
		{
			if(ContainsLoadItem(loadPath))
			{
				return loadMap[loadPath];
			}
			else return null;
		}

		public object GetLoadObj(string loadPath)
		{
			DkResourceLoadItem loadItem = GetLoadItem(loadPath);
			if(loadItem != null)
			{
				return loadItem.obj;
			}
			else return null;
		}
	
		public bool ContainsLoadItem(string loadPath)
		{
			if(loadMap.ContainsKey(loadPath))
			{
				return true;
			}
			else return false;
		}

		public void AddAssetItem(DkResourceAssetItem item)
		{
			if(!ContainsAssetItem(item.itemPath))
			{
				//Debug.Log("[DK]ResourceManage add AssetItem : "+ item.ToString());
				assetMap.Add(item.itemPath,item);
			}
		}

		protected void RemoveAssetItem(string itemPath)
		{
			DkResourceAssetItem item = GetAssetItem(itemPath);
			if(item != null)
			{
				assetMap.Remove(itemPath);
				item.Destroy();
			}
		}
		
		public DkResourceAssetItem GetAssetItem(string itemPath)
		{
			if(ContainsAssetItem(itemPath))
			{
				return assetMap[itemPath];
			}
			else return null;
		}

		public UnityEngine.Object GetAssetObject(string itemPath)
		{
			if(ContainsAssetItem(itemPath))
			{
				return assetMap[itemPath].obj;
			}
			else return null;
		}

		public Dictionary<string,DkResourceAssetItem> GetAssetDict()
		{
			return assetMap;
		}
		
		public bool ContainsAssetItem(string itemPath)
		{
			if(assetMap.ContainsKey(itemPath))
			{
				return true;
			}
			else return false;
		}
		
	}
}
