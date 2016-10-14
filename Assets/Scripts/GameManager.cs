using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour 
{
	public GameObject playerPrefab;

	public Text continueText;
	private bool blink;
	private float blinkTime = 0f;

	public Text scoreText;
	private float timeElapsed = 0f;
	private float bestTime = 0f;

	private GameObject floor;
	private GameObject water;
	private  Spawner spawner;
	private GameObject player;

	private bool gameStarted;
	private TimeManager timeManager;

	void Awake()
	{
		floor = GameObject.Find("Foreground");
		spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
		water = GameObject.Find("Water");
		timeManager = GetComponent<TimeManager>();
	}

	// Use this for initialization
	void Start () 
	{

		water.transform.position = calculateDistToCenter (water.transform);

		Vector3 posFloor = calculateDistToCenter (floor.transform);
		float defProp = posFloor.y + 30;
		floor.transform.position = new Vector3 (posFloor.x,defProp,posFloor.z);

		spawner.active = false;

		Time.timeScale = 0;

		continueText.text = "PRESS ANY BUTTON TO START";

		bestTime = PlayerPrefs.GetFloat("BestTime");
	}

	void Update ()
	{
		if(!gameStarted && Time.timeScale == 0)
		{
			if (Input.anyKeyDown) 
			{
				timeManager.ManipulateTime(1,1f);
				ResetGame();	
			}
		}

		if(!gameStarted)
		{
			blinkTime++;
			if (blinkTime % 40 == 0) 
			blink = !blink;	

			continueText.canvasRenderer.SetAlpha(blink ? 0 : 1);
		
			scoreText.text = "TIME : " + FormatTime(timeElapsed) + " \n BEST : " + FormatTime (bestTime);
		}
		else
		{
			timeElapsed += Time.deltaTime;
			scoreText.text = "TIME : " + FormatTime(timeElapsed);
		}
	}

	static Vector3 calculateDistToCenter (Transform objectChange)
	{
		var objectHeight = objectChange.localScale.y;
		var pos = objectChange.position;
		pos.x = 0;
		pos.y = -((Screen.height / PixelPerfectCamera.pixelsTounits) / 2) + (objectHeight / 2);
		return pos;
	}

	void OnPlayerKilled()
	{
		spawner.active = false;

		var playerDestroyScript = player.GetComponent<DestroyOffScreen>();
		playerDestroyScript.DestroyCallback -= OnPlayerKilled;

		player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

		timeManager.ManipulateTime(0, 5.5f);

		gameStarted = false;

		continueText.text = "PRESS ANY BUTTON TO START";

		if(timeElapsed > bestTime)
		{
			bestTime = timeElapsed;
			PlayerPrefs.SetFloat("BestTime",bestTime);
		}
	}

	void ResetGame()
	{
		spawner.active = true;

		float topScreen = (Screen.height/PixelPerfectCamera.pixelsTounits) / 2 + 100 ;
		player = GameObjectUtil.Instantiate(playerPrefab,new Vector3(0,topScreen,0));

		var playerDestroyScript = player.GetComponent<DestroyOffScreen>();
		playerDestroyScript.DestroyCallback += OnPlayerKilled;

		gameStarted = true;

		continueText.canvasRenderer.SetAlpha(0);

		timeElapsed = 0;
	}

	string FormatTime(float value)
	{
		TimeSpan t = TimeSpan.FromSeconds (value);
		return string.Format("{0:D2}:{1:D2}",t.Minutes,t.Seconds);
	}
}
