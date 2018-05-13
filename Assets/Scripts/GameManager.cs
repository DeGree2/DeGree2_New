using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public string nextLevel = "Level2";
	public int levelToUnlock = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LevelWon()
	{
		int currentLevelUnlocked = PlayerPrefs.GetInt ("levelReached", 1);
		if(currentLevelUnlocked < levelToUnlock)
			PlayerPrefs.SetInt ("levelReached", levelToUnlock);	
	}
}
