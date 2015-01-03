using UnityEngine;
using Dk.Util;

public class CameraViewTool : MonoBehaviour
{
	public float nearDistance = 0;

	public float farDistance = 0;
		
	private Camera s_Camera;
	
	public void Start()
	{
		if(this.s_Camera == null)
		{
			s_Camera = this.gameObject.GetComponent<Camera>();
		}
	}

	public void OnDrawGizmos()
	{
		if(s_Camera != null)
		{
			DrawCorners(farDistance);
			DrawCorners(nearDistance);
		}
	}

	private void DrawCorners(float distance)
	{
		if(distance != 0)
		{
			Vector3[] corners = DkCameraUtil.GetCameraView(distance, s_Camera);

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine( corners[0], corners[1]); // UpperLeft -> UpperRight
			Gizmos.DrawLine( corners[1], corners[3]); // UpperRight -> LowerRight
			Gizmos.DrawLine( corners[3], corners[2]); // LowerRight -> LowerLeft
			Gizmos.DrawLine( corners[2], corners[0]); // LowerLeft -> UpperLeft
		}
	}		
}
