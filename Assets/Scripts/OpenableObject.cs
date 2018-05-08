using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableObject : InteractionObject
{
    public bool isOpened = false; //If true, object is opened
    public bool isLocked = false; //If true, object is locked (need key to be opened)
    public GameObject key; //item needed in order to open the locked door
    private Quaternion startRotation; //rotation of an object in the beginning

    public void Start()
    {
        startRotation = transform.rotation;
    }

    //opens/closes the object
    public override void DoInteraction()
    {
        FindObjectOfType<AudioManager>().Play("OpenDoor");
        if (isOpened)
        {
            transform.rotation = startRotation; //closes door (rotation as in start rotation)
            isOpened = false;
            message.text = objectName + " was closed";
            message.SendMessage("FadeAway");
        }
        else
        {
            transform.Rotate(transform.rotation.x, transform.rotation.y - 70, transform.rotation.z); //opens door (rotates ~-70 degrees)
            isOpened = true;
            message.text = objectName + " was opened";
            message.SendMessage("FadeAway");
        }
    }
}
