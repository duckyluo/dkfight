using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dk.Util;
using Dk.Loader;
using Dk.Event;
using Dk.Asset;

namespace Dk.Loader
{
	public class DkLoadPathUtil
	{
		public static string GetDkGlobleDataPath()
		{
			return DkGlobal.GetLoadPath();
		}

		public static string GetLocalPackagePath(string packagePath)
		{
			return GetAutoLoadPath(packagePath);
		}
		
		public static string GetABPath(string path, eDkAssetPtType ptType)
		{
			string loadUrl = DkGlobal.CacheDir +"AssetBundleData/";
			if(ptType == eDkAssetPtType.ASSET_PT_IOS)
			{
				loadUrl += "IOS/";
			}
			else if(ptType == eDkAssetPtType.ASSET_PT_AND)
			{
				loadUrl += "AND/";
			}
			else if(ptType == eDkAssetPtType.ASSET_PT_PC)
			{
				loadUrl += "PC/";
			}
			else
			{
				loadUrl += "PC/";
			}

			loadUrl += path;

			return GetAutoLoadPath(loadUrl);
		}

		public static string GetConfigPath(string path)
		{
			string loadUrl = DkGlobal.CacheDir + "Config/"+path;
			return GetAutoLoadPath(loadUrl);
		}

		public static string GetTemplatePath(string path)
		{
			string loadUrl = DkGlobal.CacheDir + "Template/"+path;
			return GetAutoLoadPath(loadUrl);
		}
		
		public static string GetAutoLoadPath(string relativePath)
		{
			string loadUrl = DkFileUtil.GetFileDataPath(eFilePathType.PERSISTENT_DATA_PATH) + relativePath;
			
			if(ExistLocalFile(loadUrl))
			{
				return loadUrl;
			}
			else
			{
				loadUrl = DkFileUtil.GetFileDataPath(eFilePathType.DATA_PATH)+relativePath;
				return loadUrl;
			}
		}

		public static string GetLoadPath(string relativePath, eFilePathType pathType)
		{
			return DkFileUtil.GetFileDataPath(pathType)+relativePath;
		}

		public static bool ExistLocalFile(string path)
		{
			if(DkFileUtil.ExistFile(path))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool isSceneAsset(string path)
		{
			if(DkFileUtil.GetLastNameByFull(path) == "unity")
			{
				return true;
			}
			else return false;
		}

		public static String GetLoadPathByAssetItemPath(string path)
		{
			DkAssetItem assetItem = DkAssetListConfig.Instance.GetDkAssetItem(path);
			if(assetItem == null || string.IsNullOrEmpty(assetItem.inABName))
			{
				return null;
			}

			string abName = assetItem.inABName;
			DkABItem abItem = DkABListConfig.Instance.GetABItem(abName);

			if(abItem == null)
			{
				return null;
			}
			else
			{
				return abItem.GetLoadPath();
			}
		}

	}
}

