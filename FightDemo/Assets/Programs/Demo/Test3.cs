using UnityEngine;
using System.Collections;

public class Test3 : MonoBehaviour 
{
	

	void Start () 
	{
	
		TimerManager.Instance.Initalize();

		SceneManager.Initalize();

		SceneObjInfo info1 = new SceneObjInfo();
		info1.index = 0;
		info1.nick = "dk1";
		info1.initPos = new Vector3(2,0.1f,0);
		info1.type = eSceneObjType.Role;
		info1.team = eSceneTeamType.Me;
		SceneObj item1 = SceneManager.AddNewItem(info1);
		item1.Initalize();
		item1.Start();

		SceneObjInfo info2 = new SceneObjInfo();
		info2.index = 1;
		info2.nick = "monster1";
		info2.initPos = new Vector3(5f,0.1f,0);
		info1.type = eSceneObjType.Role;
		info2.team = eSceneTeamType.Enemy;
		SceneObj item2 = SceneManager.AddNewItem(info2);
		item2.Initalize();
		item2.Start();
	}
	

	void Update () {
	

	}
	
}
