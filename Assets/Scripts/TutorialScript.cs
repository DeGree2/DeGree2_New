using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {
    public Text message;
    private bool moved = false;
    private bool interacted = false;

    private string message1 = "Oh no! A virus made droids go MAD! You must find a way to stop them!";
    private string message2 = "Start by exploring your surroundings. Use WASD to move";
    private string message3 = "You can interact with the environment with E";
    private string message4 = "Be careful, droids are going to attack anyone on sight! Try to avoid them";
    private string message5 = "Okay, I'll stop disturbing you now. You are on your own! Good luck!";

    void Start () {
        message.text = message1; //hello message
        StartCoroutine(ChangeText());
    }

    //changes text of the tutorial message in certain order
    private IEnumerator ChangeText()
    {
        yield return new WaitForSecondsRealtime(5);
        message.text = message2; //move message
        moved = false;
        yield return new WaitForSecondsRealtime(3);
        //waits for player to move the character
        while (!moved)
        {
            yield return null;
        }
        message.text = message3; //interact message
        interacted = false;
        yield return new WaitForSecondsRealtime(3);
        //waits for player to interact with environment
        while (!interacted)
        {
            yield return null;
        }
        message.text = message4; //health message
        yield return new WaitForSecondsRealtime(7);
        message.text = message5; //bye message
        yield return new WaitForSecondsRealtime(10);
        Destroy(gameObject);
    }

    private void Update()
    {
        //checks if player has completed certain tutorial steps

        //might change later if keys change
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            moved = true;

        if (Input.GetButtonDown("Interact"))
            interacted = true;
    }
}
