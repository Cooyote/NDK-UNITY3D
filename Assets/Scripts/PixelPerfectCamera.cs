using UnityEngine;
using System.Collections;

public class PixelPerfectCamera : MonoBehaviour 
{
	public static float pixelsTounits = 1f;
	public static float scale = 1f;

	public Vector2 nativeResolution = new Vector2 (1280, 720);

	void Awake () 
	{
		var camera = GetComponent<Camera> ();

		if (camera.orthographic) 
		{
			scale = Screen.height / nativeResolution.y;
			pixelsTounits *= scale;
			camera.orthographicSize = (Screen.height/ 2) / pixelsTounits;
		}
	}
		
}
