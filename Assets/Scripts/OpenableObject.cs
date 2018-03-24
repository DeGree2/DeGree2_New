using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableObject : InteractionObject {
    public bool isOpened = false; //If true, object is opened
    public bool isLocked = false; //If true, object is locked (need key to be opened)
    public GameObject key; //item needed in order to open the locked door

    //opens/closes the object
    public override void DoInteraction()
    {
        if (isOpened)
        {
            transform.Rotate(transform.rotation.x, transform.rotation.y + 45, transform.rotation.z);
            isOpened = false;
        }
        else
        {
            transform.Rotate(transform.rotation.x, transform.rotation.y - 45, transform.rotation.z);
            isOpened = true;
        }
    }
}
