using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadScript : InteractionObject {
    public GameObject canvas; //canvas on which the keypad is on
    public string combination; //combination needed to open a door
    public GameObject affectedObject; //door that is unlocked
    private string currentComb; //combination that is constructed as the keypad buttons are pressed
    public bool used = false; //used to unlock the door (so that it wouldn't be interacted with anymore);

    public override void DoInteraction()
    {
        canvas.SetActive(true);
        canvas.GetComponent<KeypadController>().currentKeypadObject = this.gameObject;
        canvas.GetComponent<KeypadController>().isOn = true;
    }

    void Start ()
    {
        canvas.SetActive(false);
        currentComb = null;
	}

    //methods for constructing current combination
    public void Button1()
    {
        currentComb += 1;
    }
    public void Button2()
    {
        currentComb += 2;
    }
    public void Button3()
    {
        currentComb += 3;
    }
    public void Button4()
    {
        currentComb += 4;
    }
    public void Button5()
    {
        currentComb += 5;
    }
    public void Button6()
    {
        currentComb += 6;
    }
    public void Button7()
    {
        currentComb += 7;
    }
    public void Button8()
    {
        currentComb += 8;
    }
    public void Button9()
    {
        currentComb += 9;
    }
    public void Button0()
    {
        currentComb += 0;
    }
    //pressed button to leave
    public void ButtonR()
    {
        canvas.GetComponent<KeypadController>().currentKeypadObject = null;
        canvas.GetComponent<KeypadController>().isOn = false;
        canvas.GetComponent<KeypadController>().Unpause();
        canvas.SetActive(false);
        currentComb = null;
    }
    //pressed button to see if current combination is right
    public void ButtonG()
    {
        if (currentComb.Equals(combination)) //unclocked
        {
            message.text = "Keypad unlocked " + affectedObject.GetComponent<OpenableObject>().objectName.ToLower();
            message.SendMessage("FadeAway");
            affectedObject.GetComponent<OpenableObject>().isLocked = false;
            used = true;
            canvas.SetActive(false);
            canvas.GetComponent<KeypadController>().isOn = false;
            canvas.GetComponent<KeypadController>().Unpause();
        }
        else //wrong code
        {
            message.text = "Wrong code!";
            message.SendMessage("FadeAway");
            canvas.SetActive(false);
            canvas.GetComponent<KeypadController>().isOn = false;
            canvas.GetComponent<KeypadController>().Unpause();
            currentComb = null;
        }
    }
}
