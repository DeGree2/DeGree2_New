using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadController : MonoBehaviour {
    public GameObject currentKeypadObject = null; //what object opened up key pad
    public bool isOn = false;

    private void Update()
    {
        if (isOn)
        {
            Time.timeScale = 0f;
        }
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
    }

    public void Button1()
    {
        currentKeypadObject.SendMessage("Button1");
    }
    public void Button2()
    {
        currentKeypadObject.SendMessage("Button2");
    }
    public void Button3()
    {
        currentKeypadObject.SendMessage("Button3");
    }
    public void Button4()
    {
        currentKeypadObject.SendMessage("Button4");
    }
    public void Button5()
    {
        currentKeypadObject.SendMessage("Button5");
    }
    public void Button6()
    {
        currentKeypadObject.SendMessage("Button6");
    }
    public void Button7()
    {
        currentKeypadObject.SendMessage("Button7");
    }
    public void Button8()
    {
        currentKeypadObject.SendMessage("Button8");
    }
    public void Button9()
    {
        currentKeypadObject.SendMessage("Button9");
    }
    public void Button0()
    {
        currentKeypadObject.SendMessage("Button0");
    }
    public void ButtonR()
    {
        currentKeypadObject.SendMessage("ButtonR");
    }
    public void ButtonG()
    {
        currentKeypadObject.SendMessage("ButtonG");
    }


}
