using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {
    //HEA1 & HEA2 & HEA3 Živilė Valiuškaitė IFF-6/5

    Image healthBar;
    float maxHealth = 100f;
    public static float health;
    public GameObject gameOverPanel;
    public GameObject levelCompletedPanel;
    public static bool levelCompleted = false;
    public GameObject notePanel;
    public GameObject healthBarPanel;
    public GameObject cantTouchThisPanel;


    void Start () {
        healthBar = GetComponent<Image> ();
        health = maxHealth; //Starting health is max
        gameOverPanel.SetActive(false); //Game over text is disabled
        levelCompletedPanel.SetActive(false);

    }
	
	void Update () {
		healthBar.fillAmount = health/maxHealth;
        if(levelCompleted)
        {
            levelCompletedPanel.SetActive(true);
        }
        if(health<=0) //If health is 0, game is over
        {
            gameOverPanel.SetActive(true);
            notePanel.SetActive(false);
            healthBarPanel.SetActive(false);
            cantTouchThisPanel.SetActive(false);
			Time.timeScale = 0;
        }
	}
    public static void Damage()
    {
        //If damage is made, health decreases by 10
        health -= 10f;
        if(health>0)
            FindObjectOfType<AudioManager>().Play("LoseHealth");
		if (health <= 0) 
			FindObjectOfType<AudioManager> ().Play ("GameOver");
    }
    public static void TakeBonus()
    {
        //If bonus is taken, health increases by 10
        if (health < 100f) //Health can not go over max
        {
            health += 10f;
            FindObjectOfType<AudioManager>().Play("TakeItem");
        }
            
    }
    public static void AddHP(float amount)
    {
        //Adds certain amount of health points to current health
        health += amount;
        if (health > 100f) //Health can not go over max
            health = 100f;
    }
}
