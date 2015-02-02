using System;
using UnityEngine;
using Dk.Asset;
using Dk.BehaviourTree;

public class SceneRoleObj : SceneObj
{
	protected GameObject m_mainPrefab = null;

	protected GameObject m_modelPrefab = null;

	protected RoleCtrlMessage m_msgCtrl = null;
	
	protected RoleCtrlTransform m_moveCtrl = null;

	protected RoleCtrlAnimation m_aniCtrl = null;

	protected RoleCtrlSkill m_skillCtrl = null;

	protected RoleDataLocal m_localData = null;

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
				m_roleBlackBoard.DataLocal = m_localData;
				m_roleBlackBoard.DataRunTime = m_runTimeData;
				m_roleBlackBoard.CtrlAnimation = m_aniCtrl;
				m_roleBlackBoard.CtrlTransform = m_moveCtrl;
				m_roleBlackBoard.CtrlSkill = m_skillCtrl;
				m_roleBlackBoard.CtrlMsg = m_msgCtrl;
				m_roleBlackBoard.TreeDecider = m_deciderTree;
				m_roleBlackBoard.TreeExecutor = m_executorTree;

				m_aniCtrl.Initalize(m_roleBlackBoard);
				m_moveCtrl.Initalize(m_roleBlackBoard);
				m_msgCtrl.Initalize(m_roleBlackBoard);
				m_skillCtrl.Initalize(m_roleBlackBoard);
				m_deciderTree.Initalize(m_roleBlackBoard);
				m_executorTree.Initalize(m_roleBlackBoard);
				m_localData.Initalize(m_roleBlackBoard);
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
			m_mainPrefab.name = Info.nick;
			m_modelPrefab = m_mainPrefab.GetComponentInChildren<BodyUIS>().gameObject;
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
			m_localData = m_mainPrefab.AddComponent<RoleDataLocal>();
			m_runTimeData = new RoleDataRunTime();
			return true;
		}
		else return false;
	}
	
	private bool createCtrl()
	{
		m_aniCtrl = new RoleCtrlAnimation();
		m_msgCtrl = new RoleCtrlMessage();
		m_moveCtrl = new RoleCtrlTransform();
		m_skillCtrl = new RoleCtrlSkill();

		if(this.Info.team == eSceneTeamType.Me)
		{
			m_deciderTree = new RoleDeciderBt();
			m_executorTree = new RoleExecutorBt();
		}
		else
		{
			m_deciderTree = new DkBehaviourTree();
			m_executorTree = new RoleExecutorBt();

			m_mainPrefab.tag = "Enemy";
		}

		return true;
	}
	
	
	public override void Update()
	{
		if(!this.m_inited)
		{
			return;
		}

		m_deciderTree.Update(null);
		m_msgCtrl.Update();
		m_executorTree.Update(null);
		m_skillCtrl.Update();
		m_moveCtrl.Update();
	}
		
	public override void OnFsmReceive (IFsmMsg msg)
	{
		if(m_msgCtrl != null)
		{
			m_msgCtrl.AddSynFsmMsg(msg as RoleFsmMessage);
		}
	}
}