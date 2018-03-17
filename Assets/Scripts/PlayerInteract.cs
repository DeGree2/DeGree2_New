using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
    //CHA1 & CHA2 - Milda Petrikaitė IFF-6/5

    public GameObject currentInterObject = null; //current item in range
    public InteractionObject currentInterObjScript = null;
    public Inventory inventory;

    private void Update()
    {
        //if there is a current interactable object, picks it up to inventory
        if (Input.GetButtonDown("Interact") && currentInterObject)
        {
            //check to see if this object is to be stored in inventory
            if (currentInterObjScript.inventory && !currentInterObjScript.isInInventory)
            {
                inventory.AddItem(currentInterObject);
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
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //when item becomes out of range, it is not current interactable object anymore (can't pick up)
        if (other.CompareTag("interObject"))
        {
            if(other.gameObject == currentInterObject)
            {
                currentInterObject = null;
            }
        }
    }
}
