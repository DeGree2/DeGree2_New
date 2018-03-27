using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour {
    public GameObject currentInterObject = null; //current item in range
    public InteractionObject currentInterObjScript = null;
    public Inventory inventory;
    public Text message; //text of message associated with interactions

    private void Update()
    {
        //if there is a current interactable object, interacts with it
        if (Input.GetButtonDown("Interact") && currentInterObject)
        {
            //interaction specific for item (checks to see if item is to be stored in inventory and adds it)
            if (currentInterObjScript is Item)
            {
                if (((Item)currentInterObjScript).inventory && !((Item)currentInterObjScript).isInInventory)
                {
                    inventory.AddItem(currentInterObject);
                }
            }
            //interaction specific for openable object
            else if (currentInterObjScript is OpenableObject)
            {
                if (((OpenableObject)currentInterObjScript).isLocked)
                {
                    //checks if specific item is in inventory
                    if (inventory.FindItem(((OpenableObject)currentInterObjScript).key))
                    {
                        //unlocks the object
                        ((OpenableObject)currentInterObjScript).isLocked = false;
                        message.text = currentInterObjScript.objectName + " was unlocked";
                        message.SendMessage("FadeAway");
                    }
                    else
                    {
                        message.text = currentInterObjScript.objectName + " is locked";
                        message.SendMessage("FadeAway");
                    }   
                }
                //if object can be opened, opens it (or closes)
                else if (!((OpenableObject)currentInterObjScript).isLocked)
                {
                    currentInterObject.SendMessage("DoInteraction");
                }
                
            }
        }

        //changes active item to the left one from the current on inventory array
        if (Input.GetButtonDown("ActiveLeft"))
        {
            inventory.SetActiveLeft();
        }

        //changes active item to the right one from the current on inventory array
        if (Input.GetButtonDown("ActiveRight"))
        {
            inventory.SetActiveRight();
        }

        //uses currently active item from inventory
        if (Input.GetButtonDown("UseActiveItem"))
        {
            inventory.UseActive();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //when item is in range, it becomes current interactable object (can be picked up)
        if (other.CompareTag("interObject"))
        {
            currentInterObject = other.gameObject;
            currentInterObjScript = currentInterObject.GetComponent<InteractionObject>();

        }


        //Živilė. Player uses health bonus.
        if (other.CompareTag("bonusHealth"))
        {
            if (HealthBarScript.health < 100f)
            {
                other.gameObject.SetActive(false);
                HealthBarScript.TakeBonus();

                message.text = "Health restored";
                message.SendMessage("FadeAway");
            }
        }
        if (other.CompareTag("completed"))
        {
                other.gameObject.SetActive(false);

                message.text = "Level completed";
                message.SendMessage("FadeAway");
            HealthBarScript.levelCompleted = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //when item becomes out of range, it is not current interactable object anymore (can't interact)
        if (other.CompareTag("interObject"))
        {
            if(other.gameObject == currentInterObject)
            {
                currentInterObject = null;
            }
        }
    }
}
