using UnityEngine;
using System.Collections;
using Dk.Event;

namespace Dk.Asset
{
	/// <summary>
	/// Please use DkAssetLoadCollection to load asset instead;
	/// </summary>
	public class DkAssetLoadRequest :DkEventDispatch
	{
		private bool isDone = false;

		private bool isDestroyed = false;

		private GameObject loadContainer = null;

		private DkAssetLoadObject loadObj = null;

		private DkAssetItem assetItem;
		
		public DkAssetLoadRequest(DkAssetItem item)
		{
			assetItem = item;
		}
		
		override public void Destroy()
		{
			if(isDestroyed == false)
			{
				isDestroyed = true;
				GameObject.Destroy(loadContainer);

				loadContainer = null;
				loadObj = null;
				assetItem = null;

				base.Destroy();
			}
		}

		public bool GetIsDone
		{
			get
			{
				return isDone;
			}
		}

		public DkAssetItem GetAssetItem
		{
			get
			{
				return assetItem;
			}
		}
		
		public void StartLoad()
		{
			if(loadContainer == null && isDone == false)
			{
				loadContainer = new GameObject();
				loadContainer.name = "loadContainer";
				loadObj = loadContainer.AddComponent(typeof(DkAssetLoadObject)) as DkAssetLoadObject;
				loadObj.req = this;

				loadObj.StartLoad();
			}
		}
		
		public void Finish()
		{
			if(isDone == false)
			{
				isDone = true;
				DispatchEvent(new DkEvent(DkEventDef.LOAD_COMPLETE));
			}
		}
	}
}
