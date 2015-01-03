using System;
using UnityEngine;
using Dk.Asset;
using Dk.BehaviourTree;

public class SceneRoleObj : SceneObj
{
	protected GameObject m_mainPrefab = null;

	protected GameObject m_modelPrefab = null;

	protected RoleCtrlMsg m_msgCtrl = null;
	
	protected RoleCtrlMove m_moveCtrl = null;

	protected RoleCtrlAnimation m_aniCtrl = null;

	protected RoleDataBase m_baseData = null;

	protected RoleDataRunTime m_runTimeData = null;

	protected DkBehaviourTree m_deciderTree = null;

	protected DkBehaviourTree m_executorTree = null;

	protected RoleBlackBoard m_roleBlackBoard = null;

	public SceneRoleObj(SceneObjInfo info):base(info)
	{

	}

	public override void Destroy()
	{
		
	}
	
	public override void Initalize()
	{
		if(this.m_inited == false)
		{
			bool bol = true;
			bol = (bol != true ? bol : createPrefab());
			bol = (bol != true ? bol : createData());
			bol = (bol != true ? bol : createCtrl());
			
			if(bol)
			{
				m_roleBlackBoard = new RoleBlackBoard();

				m_roleBlackBoard.PrefabMain = m_mainPrefab;
				m_roleBlackBoard.PrefabModel = m_modelPrefab;
				m_roleBlackBoard.DataInfo = m_info;
				m_roleBlackBoard.DataBase = m_baseData;
				m_roleBlackBoard.DataRunTime = m_runTimeData;
				m_roleBlackBoard.CtrlAnimation = m_aniCtrl;
				m_roleBlackBoard.CtrlMove = m_moveCtrl;
				m_roleBlackBoard.CtrlMsg = m_msgCtrl;
				m_roleBlackBoard.TreeDecider = m_deciderTree;
				m_roleBlackBoard.TreeExecutor = m_executorTree;

				m_aniCtrl.Initalize(m_roleBlackBoard);
				m_moveCtrl.Initalize(m_roleBlackBoard);
				m_msgCtrl.Initalize(m_roleBlackBoard);
				(m_deciderTree as RoleDeciderBt).Initalize(m_roleBlackBoard);
				(m_executorTree as RoleExecutorBt).Initalize(m_roleBlackBoard);
			}
			else
			{
				Debug.Log("[error] role inited failed");
			}
			
			this.m_inited = bol;
		}
	}

	private bool createPrefab()
	{

		String path = "Assets/SourceData/Prefabs/Npc/PC/100001.prefab"; //"Dk.prefab";
		UnityEngine.Object prefab = Resources.LoadAssetAtPath(path,typeof(GameObject));
		if(prefab != null)
		{
			m_mainPrefab = GameObject.Instantiate(prefab) as GameObject;
			m_mainPrefab.name = "Dk1";
			m_modelPrefab = GameObject.Find("/Dk1/Body");
			return true;
		}
		else
		{
			Debug.Log("[error]  can not find prefab , by path "+path);
			return false;
		}
	}
	
	private bool createData()
	{
		if(m_mainPrefab != null)
		{
			m_baseData = m_mainPrefab.GetComponent<RoleDataBase>();
			m_runTimeData = new RoleDataRunTime();
			return true;
		}
		else return false;
	}
	
	private bool createCtrl()
	{
		m_aniCtrl = new RoleCtrlAnimation();
		m_msgCtrl = new RoleCtrlMsg();
		m_moveCtrl = new RoleCtrlMove();

		if(this.Info.isMe)
		{
			m_deciderTree = new RoleDeciderBt();
			m_executorTree = new RoleExecutorBt();
		}
		return true;
	}
	
//	public override void Start()
//	{
//		if(!this.m_inited)
//		{
//			return;
//		}
//	}
	
	public override void Update()
	{
		if(!this.m_inited)
		{
			return;
		}

		m_deciderTree.Update(null);
		m_msgCtrl.Update();
		m_executorTree.Update(null);
		//m_aniCtrl.Update();
		m_moveCtrl.Update();
	}
		
	
	public override void OnFsmReceive (ISceneFsmMsg msg)
	{
		if(m_msgCtrl != null)
		{
			m_msgCtrl.AddSynFsmMsg(msg as RoleFsmMessage);
		}
	}
}