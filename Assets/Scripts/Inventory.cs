using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Inventory : MonoBehaviour {
    public GameObject[] inventory = new GameObject[10];//inventory array to store items
    public Button[] InventoryButtons = new Button[10]; //array for display
    public int activeSlot = -1; //active item slot, -1 means not selected

    public void Start()
    {
        //making sure inventory is empty in the beginning
        for(int i = 0; i<10; i++)
        {
            inventory[i] = null;
        }
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
                Debug.Log(item.name + " was added");
                itemAdded = true;
                //do something with the object
                item.SendMessage("DoInteraction");
                break;
            }
        }

        //inventory was full
        if (!itemAdded)
        {
            Debug.Log("Inventory full");
        }
    }

    public bool FindItem(GameObject item)
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == item)
                return true; //item found
        }
        return false; //item not present in inventory
    }

    //sets slot as active left from currently active
    public void SetActiveLeft()
    {
        //if none selected yet, sets as last
        if (activeSlot == -1)
            activeSlot = inventory.Length - 1;
        //changes currently active slot to normal color and calculates the next slot
        else
        {
            InventoryButtons[activeSlot].GetComponent<Image>().color = Color.white;
            activeSlot--;
            if (activeSlot == -1)
                activeSlot = activeSlot + inventory.Length;
        }
        //changes the color of currently active slot
        InventoryButtons[activeSlot].GetComponent<Image>().color = Color.grey;
    }

    //sets slot as active right from currently active
    public void SetActiveRight()
    {
        //if none selected yet, sets as first
        if (activeSlot == -1)
            activeSlot = 0;
        //changes currently active slot to normal color and calculates the next slot
        else
        {
            InventoryButtons[activeSlot].GetComponent<Image>().color = Color.white;
            activeSlot = (activeSlot + 1) % inventory.Length;
        }
        //changes the color of currently active slot
        InventoryButtons[activeSlot].GetComponent<Image>().color = Color.grey;
    }

    //decreases usability of item
    public void UseActive()
    {
        if(activeSlot != -1 && inventory[activeSlot] != null)
        {
            inventory[activeSlot].SetActive(true);
            inventory[activeSlot].SendMessage("Use");
            //if item was deactivated, usability is 0, so it is removed from inventory
            if (!inventory[activeSlot].activeInHierarchy)
            {
                Debug.Log(inventory[activeSlot].name + " was used and reached its usability limit");
                InventoryButtons[activeSlot].GetComponentInChildren<Text>().text = "";
                inventory[activeSlot] = null;
            }
            else
            {
                inventory[activeSlot].SetActive(false);
                Debug.Log(inventory[activeSlot].name + " was used but has not reached its usability limit");
            } 
        }
    }
}
