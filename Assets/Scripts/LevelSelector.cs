using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

	public Button[] levelButtons;

	void Start()
	{
		int levelReached = PlayerPrefs.GetInt ("levelReached", 1);

		for (int i = 0; i < levelButtons.Length; i++) 
		{
			if(i + 1 > levelReached)
				levelButtons [i].interactable = false;
		}
	}

	public void LoadLevel(string levelName)
	{
		SceneManager.LoadScene (levelName);
	}

	public void ResetLevels()
	{
		PlayerPrefs.SetInt ("levelReached", 1);
	
		for (int i = 1; i < levelButtons.Length; i++) 
			levelButtons [i].interactable = false;
	}

	public void UnlockAll()
	{
		PlayerPrefs.SetInt ("levelReached", 5);

		for (int i = 0; i < levelButtons.Length; i++) 
			levelButtons [i].interactable = true;
	}
}
