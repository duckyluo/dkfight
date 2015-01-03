using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dk.Util;

public class Test4 : MonoBehaviour {

	public static List<Test3> npcs = new List<Test3>();

	public Transform target;

	public static DkSerializeData data;

	// Use this for initialization
	public void Awake () 
	{
		npcs.Clear();
	}

	public void Start()
	{
		//data = ScriptableObject.CreateInstance<DkSerializeData>();
		//data = Object.Instantiate(new DkSkillData()) as DkSerializeData;
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(0,0,100,50),"Serialieze"))
		{
			//Hero hero = new Hero();
			//data.DoSerialize(hero);
		}

//		if(GUI.Button(new Rect(110,0,100,50),"ChangeName"))
//		{
//			//data.ChangeName("God");
//		}
	
		if(GUI.Button(new Rect(220,0,100,50),"DeSerialieze"))
		{
			//Hero obj = data.GetDeSerialiezeObj() as Hero;
			//Debug.Log(obj.ToString());
		}

//		if(GUI.Button(new Rect(330,0,100,50),"Show"))
//		{
//			//Debug.Log(test.GetHeroString());
//		}
		
	}
	
	// Update is called once per frame
	public void Update ()  
	{
//		if(Input.GetMouseButtonDown(0))
//		{
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//			RaycastHit hit;
//			if(Physics.Raycast(ray,out hit))
//			{
//				Vector3 point = hit.point;
//				foreach(Test3 npc in npcs)
//				{
//					npc.Goto(point);
//				}
//				if(target)
//				{
//					target.position = point;
//				}
//			}
//		}
	}
}
