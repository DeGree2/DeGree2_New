using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Inventory : MonoBehaviour {
    public GameObject[] inventory = new GameObject[10];//inventory array to store items
    public Button[] InventoryButtons = new Button[10]; //array for display
    public int activeSlot = -1; //active item slot, -1 means not selected
    public Text message; //text of message associated with inventory
    public bool enemyDamaged;  //vital for enemyBehavior

    public void Start()
    {
        //making sure inventory is empty in the beginning
        for(int i = 0; i < 10; i++)
        {
            inventory[i] = null;
        }
    }

    public void SlotSelect(int index)
    {
        if (inventory[index] != null && activeSlot != index) //selects if slot is occupied and different from active
        {
            if(activeSlot != -1)
            {
                InventoryButtons[activeSlot].GetComponent<Image>().color = Color.white;
            }
            activeSlot = index;
            InventoryButtons[index].GetComponent<Image>().color = Color.grey;
        }
        else //deselects if selected the same button or if slot is empty
        {
            InventoryButtons[index].GetComponent<Image>().color = Color.white;
            activeSlot = -1;
        }
        Debug.Log(activeSlot);
    }

    public void AddItem(GameObject item)
    {
        bool itemAdded = false; //to determine whether the item was added (for inv is full case)
        //find the first unoccupied slot in the inventory
        for(int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i] == null)
            {
                //add item
                inventory[i] = item;
                //update UI
                InventoryButtons[i].GetComponentInChildren<Text>().text = "+";
                item.SendMessage("UpdateUI");
                itemAdded = true;
                //do something with the object
                item.SendMessage("DoInteraction");
                break;
            }
        }

        //inventory was full
        if (!itemAdded)
        {
            message.text = "Inventory full";
            message.SendMessage("FadeAway");
        }
    }

    public bool FindItem(GameObject item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == item)
                return true; //item found
        }
        return false; //item not present in inventory
    }

    //decreases usability of item
    public void UseActive(GameObject enemyInRange)
    {
        if(activeSlot != -1 && inventory[activeSlot] != null)
        {
            inventory[activeSlot].SetActive(true);
            Item temp = inventory[activeSlot].GetComponent<Item>();
            if (temp.weapon) //if item is weapon, using it destroys enemy
            {
                enemyDamaged = false;   //no touching pls :)
                if (enemyInRange)
                {
                    inventory[activeSlot].SendMessage("Use");
                    Debug.Log("Killed enemy " + enemyInRange.name); //enemyInRange.sendMessage("Die"); ?
                    enemyDamaged = true;    //no touching pls :)
                }
                else
                {
                    message.text = "No enemy in range!";
                    message.SendMessage("FadeAway");
                }
            }
            else
                inventory[activeSlot].SendMessage("Use");
            
            //if item was deactivated, usability is 0, so it is removed from inventory
            if (!inventory[activeSlot].activeInHierarchy)
            {
                Debug.Log(inventory[activeSlot].name + " was used and reached its usability limit");
                InventoryButtons[activeSlot].GetComponentInChildren<Text>().text = "";
                inventory[activeSlot] = null;
                SlotSelect(activeSlot);
            }
            else
            {
                inventory[activeSlot].SetActive(false);
                Debug.Log(inventory[activeSlot].name + " was used but has not reached its usability limit");
            }
        }
    }

    public void DropActive()
    {
        if (activeSlot != -1 && inventory[activeSlot] != null)
        {
            GameObject drop = inventory[activeSlot];
            drop.SetActive(true);
            //chooses position close to player for dropping
            Vector3 charPosition = gameObject.transform.position;
            charPosition.y += 1f;
            Vector3 charDirection = gameObject.transform.forward;
            Vector3 dropPosition = charPosition + charDirection * 0.7f;

            drop.transform.position = dropPosition;
            //drop.GetComponent<Rigidbody>().AddForce(dropPosition * 0.5f);
            drop.SendMessage("Drop"); //resets ability to be stored in inventory again
            //resets invenotory slot
            InventoryButtons[activeSlot].GetComponentInChildren<Text>().text = "";
            inventory[activeSlot] = null;
            SlotSelect(activeSlot);
        }
    }

    public void ThrowActive()
    {
        if (activeSlot != -1 && inventory[activeSlot] != null)
        {
            GameObject drop = inventory[activeSlot];
            drop.SetActive(true);
            //chooses position close to player for dropping, then for throwing
            Vector3 charPosition = gameObject.transform.position;
            charPosition.y += 1.5f;
            Vector3 charDirection = gameObject.transform.forward;
            Vector3 dropPosition = charPosition + charDirection * 1f;
            Vector3 throwPosition = charPosition + charDirection * 550f;

            drop.transform.position = dropPosition;
            drop.GetComponent<Rigidbody>().AddForce(throwPosition);
            drop.SendMessage("Drop"); //resets ability to be stored in inventory again
            //resets invenotory slot
            InventoryButtons[activeSlot].GetComponentInChildren<Text>().text = "";
            inventory[activeSlot] = null;
            SlotSelect(activeSlot);
        }
    }
}
