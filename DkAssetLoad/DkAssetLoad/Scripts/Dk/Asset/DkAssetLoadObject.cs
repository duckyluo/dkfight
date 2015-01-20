using UnityEngine;
using System.Collections;

namespace Dk.Asset
{
	public class DkAssetLoadObject : MonoBehaviour
	{
		public DkAssetLoadRequest req;
		
		private Object obj;

		private AssetBundle ab;

		public void Awake()
		{
			DontDestroyOnLoad(this);
		}
		
		public void OnDestroy()
		{
			this.req = null;
			this.obj = null;
			this.ab = null;
		}

		public UnityEngine.Object GetLoadObj()
		{
			return obj;
		}

		public void StartLoad()
		{
			if(req == null || req.GetAssetItem == null)
			{
				Debug.Log("[error][DkAssetLoadObject] : req or req.GetAssetItem is null");
				return;
			}

			if(DkResourceManager.Instance.GetAssetObject(req.GetAssetItem.path) != null)
			{
				obj = DkResourceManager.Instance.GetAssetObject(req.GetAssetItem.path);
				req.Finish();
				return;
			}

			if(req.GetAssetItem.assetType == (int)eDkAssetSourceType.SRC_TYPE_SCENE)
			{
				StartCoroutine(LoadAssetInScene());
			}
			else if(DkGlobal.GetSourceDataEnable()) 
			{
				StartCoroutine(LoadAssetInPath());
			}
			else
			{
				string abName = req.GetAssetItem.inABName;
				DkABItem abItem = DkABListConfig.Instance.GetABItem(abName);
				if(abItem != null)
				{
					DkResourceLoadItem loadItem = DkResourceManager.Instance.GetLoadItem(abItem.GetLoadPath());
					if(loadItem != null && loadItem.obj != null)
					{
						this.ab = (AssetBundle)loadItem.obj;
						StartCoroutine(LoadAssetInAssetBundle());
					}
					else
					{
						Debug.Log("[error][DkAssetLoadObject StartLoad] can not find assetBundle : "+abName);
						req.Finish();
					}
				}
				else
				{
					Debug.Log("[error][DkAssetLoadObject StartLoad] something wrong in AssetConfigItem : "+abItem.path);
				}
			}
		}

		IEnumerator LoadAssetInScene()
		{
			yield return null;
			DkResourceAssetItem resourceItem = new DkResourceAssetItem();
			resourceItem.itemPath = req.GetAssetItem.path;
			resourceItem.assetType = (eDkAssetSourceType)req.GetAssetItem.assetType;
			resourceItem.obj = null;

			DkResourceManager.Instance.AddAssetItem(resourceItem);
			req.Finish();
		}

		IEnumerator LoadAssetInPath()
		{
			System.Type type = req.GetAssetItem.GetAssetType;
			obj = Resources.LoadAssetAtPath(req.GetAssetItem.AssetPath,type);
			yield return null;
		
			if(obj != null)
			{
				DkResourceAssetItem resourceItem = new DkResourceAssetItem();
				resourceItem.itemPath = req.GetAssetItem.path;
				resourceItem.assetType = (eDkAssetSourceType)req.GetAssetItem.assetType;
				resourceItem.obj = obj;
				
				DkResourceManager.Instance.AddAssetItem(resourceItem);
			}
			req.Finish();
		}
		
		IEnumerator LoadAssetInAssetBundle()
		{
			AssetBundleRequest abRequest = ab.LoadAsync(req.GetAssetItem.guid,req.GetAssetItem.GetAssetType);
			yield return abRequest;

			obj = abRequest.asset;

			if(obj != null)
			{
				DkResourceAssetItem resourceItem = new DkResourceAssetItem();
				resourceItem.itemPath = req.GetAssetItem.path;
				resourceItem.assetType = (eDkAssetSourceType)req.GetAssetItem.assetType;
				resourceItem.obj = obj;
				
				DkResourceManager.Instance.AddAssetItem(resourceItem);
			}
			else
			{
				Debug.Log("[error] load asset from assetBundle failed, path is : "+req.GetAssetItem.path);
			}

			req.Finish();
		}
		
	}
}
