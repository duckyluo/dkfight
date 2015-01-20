using UnityEngine;

namespace Dk.Util
{
	public class DkCameraUtil
	{		

		public static Vector2 GetViewWH(float distance, Camera camera)
		{
			Transform tx = camera.transform;
			
			float halfFOV;
			float width;
			float height;

			if(camera.isOrthoGraphic)
			{
				halfFOV = camera.orthographicSize;
				width = halfFOV*camera.aspect;
				height = halfFOV;

				return new Vector2(width,height);
			}
			else
			{
			 	halfFOV = ( camera.fieldOfView * 0.5f ) * Mathf.Deg2Rad;
				float aspect = camera.aspect;
				height = distance * Mathf.Tan( halfFOV );
				width = height * aspect;

				return new Vector2(width,height);
			}
		}
		

		public static Vector3[] GetCameraView(float distance, Camera camera)
		{
			if(camera.isOrthoGraphic)
			{
				return GetOrthographicView(distance,camera);
			}
			else
			{
				return GetPerspectiveView(distance,camera);
			}
		}
		
		private static Vector3[] GetOrthographicView(float distance , Camera camera)
		{
			Transform tx = camera.transform;
			
			float halfFOV = camera.orthographicSize;
			float width = halfFOV*camera.aspect;
			float height = halfFOV;
			
			return GetCorners(width,height,distance,tx);
		}
		
		private static Vector3[] GetPerspectiveView(float distance , Camera camera)
		{
			Transform tx = camera.transform;
			float halfFOV = ( camera.fieldOfView * 0.5f ) * Mathf.Deg2Rad;
			float aspect = camera.aspect;
			
			float height = distance * Mathf.Tan( halfFOV );
			float width = height * aspect;
			
			return GetCorners(width,height,distance,tx);
		}
		
		private static Vector3[] GetCorners(float width, float height, float distance, Transform tx)
		{
			Vector3[] corners = new Vector3[ 4 ];
			
			// UpperLeft
			corners[ 0 ] = tx.position - ( tx.right * width );
			corners[ 0 ] += tx.up * height;
			corners[ 0 ] += tx.forward * distance;
			
			// UpperRight
			corners[ 1 ] = tx.position + ( tx.right * width );
			corners[ 1 ] += tx.up * height;
			corners[ 1 ] += tx.forward * distance;
			
			// LowerLeft
			corners[ 2 ] = tx.position - ( tx.right * width );
			corners[ 2 ] -= tx.up * height;
			corners[ 2 ] += tx.forward * distance;
			
			// LowerRight
			corners[ 3 ] = tx.position + ( tx.right * width );
			corners[ 3 ] -= tx.up * height;
			corners[ 3 ] += tx.forward * distance;
			
			return corners;
		}
	}
}
