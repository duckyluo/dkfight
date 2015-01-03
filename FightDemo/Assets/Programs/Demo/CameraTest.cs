using UnityEngine;
using System.Collections;
using Dk.Util;

public class CameraTest : MonoBehaviour 
{		
	public Camera m_camera;

	public Transform hero;

	public GameObject leftBox;

	public GameObject rightBox;

	private Vector3[] lineV = null;
	
	public void OnDrawGizmos()
	{
		if(lineV != null)
		{
			DrawLine();
		}
	}
	
	void Start () 
	{
		if(hero != null && this.m_camera != null)
		{		
			float dis = hero.position.z - m_camera.transform.position.z;

			Vector2 vect = GetPerspectiveView(dis,m_camera);
			Vector3 leftPoint = m_camera.transform.position;
			leftPoint.x = leftBox.transform.position.x + vect.x ;
			Vector3 RightPoint =  m_camera.transform.position;
			RightPoint.x = rightBox.transform.position.x - vect.x;

			lineV = new Vector3[2]{leftPoint,RightPoint};
		}
	}

	void Update () 
	{
		
	}

	private void DrawLine()
	{
		if(lineV != null && lineV.Length == 2)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(lineV[0],lineV[1]);
		}
	}

	private Vector2 GetPerspectiveView(float distance , Camera camera)
	{
		Transform tx = camera.transform;
		float halfFOV = ( camera.fieldOfView * 0.5f ) * Mathf.Deg2Rad;
		float aspect = camera.aspect;
		
		float height = distance * Mathf.Tan( halfFOV );
		float width = height * aspect;
		
		return new Vector2(width,height);
	}
}
