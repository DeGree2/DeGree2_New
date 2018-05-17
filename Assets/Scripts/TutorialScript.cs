using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {
    public Text message;
    private bool moved = false;
    private bool interacted = false;
    public GameObject panel1; //other tutorial panel (near health)
    public GameObject panel2; //other tutorial panel (near inventory)
    private bool paused = false;

    private string message1 = "Oh no! A virus made droids go MAD! You must find a way to stop them!";
    private string message2 = "Start by exploring your surroundings. Use WASD to move";
    private string message3 = "You can interact with the environment (pick up items, open doors and so on) with E";
    private string message32 = "Items are stored in inventory. Whenever you want to use an item, just select the slot and press E! To drop it, press Q instead!";
    private string message4 = "Be careful, droids are going to attack anyone on sight! Try to avoid them because they can hurt you!";
    private string message42 = "Your health bar shows how alive you are; enemies can decrease you health but various items can increase it";
    private string message5 = "Okay, I'll stop disturbing you now. Try to find a way to stop this madness! You are on your own! Good luck!";

    void Start () {
        message.text = message1; //hello message
        StartCoroutine(ChangeText());
        panel1.SetActive(false);
        panel2.SetActive(false);
    }

    //changes text of the tutorial message in certain order
    private IEnumerator ChangeText()
    {
        yield return new WaitForSecondsRealtime(4);

        while (paused)
            yield return null;

        message.text = message2; //move message
        moved = false;
        yield return new WaitForSecondsRealtime(2);
        //waits for player to move the character
        while (!moved)
        {
            yield return null;
        }

        while (paused)
            yield return null;

        message.text = message3; //interact message
        interacted = false;
        yield return new WaitForSecondsRealtime(2);
        //waits for player to interact with environment
        while (!interacted)
        {
            yield return null;
        }

        while (paused)
            yield return null;

        message.text = message32; //about inventory
        //makes inventory panel blink
        for (int i = 0; i < 8; i++)
        {
            while (paused)
                yield return null;

            panel2.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);

            while (paused)
                yield return null;

            panel2.SetActive(false);
            yield return new WaitForSecondsRealtime(0.5f);
        }

        while (paused)
            yield return null;

        message.text = message4; //about droids attacking you
        yield return new WaitForSecondsRealtime(6);

        while (paused)
            yield return null;

        message.text = message42; //about health
        //makes health panel blink
        for (int i = 0; i < 8; i++)
        {
            while (paused)
                yield return null;

            panel1.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);

            while (paused)
                yield return null;

            panel1.SetActive(false);
            yield return new WaitForSecondsRealtime(0.5f);
        }

        while (paused)
            yield return null;

        message.text = message5; //bye message
        yield return new WaitForSecondsRealtime(5);
        Destroy(gameObject);
    }

    private void Update()
    {
        //checks if player has completed certain tutorial steps

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            moved = true;

        if (Input.GetButtonDown("Interact"))
            interacted = true;

        if(Time.timeScale == 0)
        {
            paused = true;
        }
        else
        {
            paused = false;
        }
    }
}
