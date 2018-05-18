using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : InteractionObject
{
    public bool isLocked = false; //If true, object is locked (need key or event to be unlocked)
    public GameObject key; //item needed to unlock
    public GameObject affectedObject; //object that is affected by interaction with this object
    public int type = 0; //type of event when interacted
    //type 0 - default; no functionality for object (needed to make objects which have already done their job uninteractable)
    //type 1 - door unlocker;
    //type 2 - other eventObject unlocker (only affects their type, not being locked with items);
    //type 3 - wall/... remover;
    public int changeToType = 0; //becomes object type if unlocked by type 2 EventObject;
    public Material working;
    public Material notWorking;
    public Material locked;

    private void Start()
    {
        Material mat = gameObject.GetComponent<MeshRenderer>().materials[0];
        if (type == 0)
        {
            gameObject.GetComponent<MeshRenderer>().materials = new Material[2] { mat, notWorking };
        }
        else if (isLocked)
        {
            gameObject.GetComponent<MeshRenderer>().materials = new Material[2] { mat, locked };
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().materials = new Material[2] { mat, working };
        }
    }

    public override void DoInteraction()
    {
        if (type == 1)
        {
            OpenableObject affectedObjectScript = affectedObject.GetComponent<OpenableObject>();
            affectedObjectScript.isLocked = false; //unlocks affected object
            message.text = objectName + " used to unlock " + affectedObjectScript.objectName.ToLower();
            message.SendMessage("FadeAway");
            type = 0; //makes object uninteractable
            FindObjectOfType<AudioManager>().Play("console");
        }
        else if (type == 2)
        {
            EventObject affectedObjectScript = affectedObject.GetComponent<EventObject>();
            affectedObjectScript.type = affectedObjectScript.changeToType; //makes affected object interactable
            affectedObjectScript.MadeUsable();
            message.text = objectName + " made " + affectedObjectScript.objectName.ToLower() + " usable";
            message.SendMessage("FadeAway");
            type = 0; //makes object uninteractable
            FindObjectOfType<AudioManager>().Play("console");
        }
        else if(type == 3)
        {
            affectedObject.SetActive(false); //removes affected object
            message.text = objectName + " used to remove some obstacles";
            message.SendMessage("FadeAway");
            type = 0; //makes object uninteractable
            FindObjectOfType<AudioManager>().Play("console");
            FindObjectOfType<AudioManager>().Play("explode");
        }
    }

    public void Unlocked()
    {
        Material mat = gameObject.GetComponent<MeshRenderer>().materials[0];
        gameObject.GetComponent<MeshRenderer>().materials = new Material[2] { mat, working };
        FindObjectOfType<AudioManager>().Play("repair");
    }

    public void MadeUsable()
    {
        Material mat = gameObject.GetComponent<MeshRenderer>().materials[0];
        if (isLocked)
        {
            gameObject.GetComponent<MeshRenderer>().materials = new Material[2] { mat, locked };
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().materials = new Material[2] { mat, working };
        }
        
    }
}
