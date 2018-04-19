﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour {
    public GameObject currentInterObject = null; //current item in range
    public InteractionObject currentInterObjScript = null;
    public GameObject enemyInRange = null; //current enemy in range
    public Inventory inventory;
    public Text message; //text of message associated with interactions
    Animator anim;
    string layerName = "ThrownObject";
    int layerIndex;
    

    private void Start()
    {
        layerIndex = LayerMask.NameToLayer(layerName);
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //if there is a current interactable object, interacts with it
        if (Input.GetButtonDown("Interact"))
        {
            if (inventory.HasActive())
            {
                inventory.UseActive(enemyInRange);
            }
            else if (currentInterObject)
            {
                //interaction specific for item (checks to see if item is to be stored in inventory and adds it)
                if (currentInterObjScript is Item)
                {
                    if (((Item)currentInterObjScript).inventory && !((Item)currentInterObjScript).isInInventory)
                    {
                        inventory.AddItem(currentInterObject);
                        anim.SetTrigger("isPickingUp");
                        currentInterObject.layer = layerIndex;
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
                            message.text = currentInterObjScript.objectName + " unlocked with " + ((OpenableObject)currentInterObjScript).key.GetComponent<Item>().objectName.ToLower();
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
                else if (currentInterObjScript is EventObject)
                {
                    //checks if object can be interacted with (type = 0 means object is uninteractable)
                    if(((EventObject)currentInterObjScript).type != 0)
                    {
                        if (((EventObject)currentInterObjScript).isLocked)
                        {
                            //checks if specific item is in inventory
                            if (inventory.FindItem(((EventObject)currentInterObjScript).key))
                            {
                                //unlocks the object
                                ((EventObject)currentInterObjScript).isLocked = false;
                                message.text = "You can now use " + currentInterObjScript.objectName.ToLower();
                                message.SendMessage("FadeAway");
                            }
                            else
                            {
                                message.text = "Some item needed to use " + currentInterObjScript.objectName.ToLower();
                                message.SendMessage("FadeAway");
                            }
                        }
                        //if object can be interacted with, interacts
                        else if (!((EventObject)currentInterObjScript).isLocked)
                        {
                            currentInterObject.SendMessage("DoInteraction");
                        }
                    }
                }
            }
        }

        //drops currently active item from inventory
        if (Input.GetButtonDown("Drop"))
        {
            inventory.DropActive();
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

        //notices enemy in range (needed for ability to kill robot)
        if (other.CompareTag("enemy"))
        {
            enemyInRange = other.gameObject;
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

        if (other.CompareTag("enemy"))
        {
            if (other.gameObject == enemyInRange)
            {
                enemyInRange = null;
            }
        }
    }
}
