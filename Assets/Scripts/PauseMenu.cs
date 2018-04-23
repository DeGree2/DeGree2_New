﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool gameIsPaused = false;
    public static bool inOptions = false;

    public GameObject pauseMenuUI;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !inOptions)
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && inOptions)
        {
            GameObject.Find("OptionsMenu").SetActive(false);
            inOptions = false;
            pauseMenuUI.SetActive(true);
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

	public void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OptionsMenu()
    {
        if (inOptions == false)
            inOptions = true;
        else
            inOptions = false;
    }
}
