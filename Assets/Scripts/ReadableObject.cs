using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadableObject : InteractionObject
{
    public GameObject panel; //panel which is shown on screen when object is interacted with
    public Sprite noteImage; //image of "note" that will be shown on screen when reading

    private void Start()
    {
        panel.SetActive(false);
    }

    public override void DoInteraction()
    {
        panel.GetComponentInChildren<Image>().sprite = noteImage; //changes the image to note image
        panel.SetActive(true);
    }
}
