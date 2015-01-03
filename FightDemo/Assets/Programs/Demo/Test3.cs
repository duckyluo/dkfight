using UnityEngine;
using System.Collections;

public class Test3 : MonoBehaviour 
{
	

	void Start () 
	{
	
		TimerManager.Instance.Initalize();

		SceneManager.Initalize();

		SceneObjInfo info = new SceneObjInfo();
		SceneObj item = SceneManager.AddNewItem(info);

		item.Initalize();
		item.Start();
	}
	

	void Update () {
	

	}
	
}
