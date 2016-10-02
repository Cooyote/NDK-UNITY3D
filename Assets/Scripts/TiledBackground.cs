using UnityEngine;
using System.Collections;

public class TiledBackground : MonoBehaviour 
{
	public Vector2 textureSize = new Vector2(1 , 1);


	public bool scaleHorizontialy = true;
	public bool scaleVerticaly = true;

	void Start () 
	{
		var newWidth =!scaleHorizontialy ? 1 : Mathf.Ceil (Screen.width / (textureSize.x * PixelPerfectCamera.scale));
		var newHeight =!scaleVerticaly ? 1 : Mathf.Ceil (Screen.height / (textureSize.y * PixelPerfectCamera.scale));

		transform.localScale = new Vector3 (newWidth * textureSize.x, newHeight * textureSize.y, 1);

		GetComponent<Renderer> ().material.mainTextureScale = new Vector3 (newWidth, newHeight, 1);
	}

}
