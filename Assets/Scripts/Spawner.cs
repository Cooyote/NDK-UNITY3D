using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
	public GameObject[] prefabs;
	public float delay = 5.0f;
	public bool active = true;
	public Vector2 delayRange = new Vector2(2.8f,3.5f);


	void Start () 
	{
		StartCoroutine (PlatformGenerator ());
	}


	IEnumerator PlatformGenerator()
	{
		yield return new WaitForSeconds (delay);
		if (active) 
		{
			var newTransform = transform;
			GameObjectUtil.Instantiate (prefabs [Random.Range (0, prefabs.Length)], newTransform.position);
			ResetDelay ();
		}
		StartCoroutine (PlatformGenerator ());
	}

	void ResetDelay()
	{
		delay = Random.Range (delayRange.x, delayRange.y);
	}
}
