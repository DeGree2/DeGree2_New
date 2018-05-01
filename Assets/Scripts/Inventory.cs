using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Inventory : MonoBehaviour {
    public GameObject[] inventory = new GameObject[10];//inventory array to store items
    public Button[] InventoryButtons = new Button[10]; //array for display
    public int activeSlot = -1; //active item slot, -1 means not selected
    public Text message; //text of message associated with inventory

    [HideInInspector]
    public bool enemyDamaged;  //vital for enemyBehavior
    [HideInInspector]
    public bool enemyInRange;  //vital for enemyBehavior
    GameObject[] enemies;
    RaycastHit hit;
    public bool damageOnly1;
    public int damageRange;
    bool inRange;

    public void Start()
    {
        //making sure inventory is empty in the beginning
        for(int i = 0; i < 10; i++)
        {
            inventory[i] = null;
        }
    }

    public void Update()
    {
        enemies = null;
        enemies = GameObject.FindGameObjectsWithTag("enemy");

        int countInRange = 0;

        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) <= damageRange)
            {
                countInRange++;
            }

        }

        if (countInRange == 0)
        {
            inRange = false;
        }

        
        foreach (GameObject enemy in enemies)
        {
            if (Physics.Linecast(transform.position, enemy.transform.position, out hit))
            {
                if (hit.transform.tag == "enemy")
                {
                    if (Vector3.Distance(enemy.transform.position, transform.position) <= damageRange)
                    {
                        inRange = true;
                    }
                }
            }

        }
    }

    //marks/unmarks slot as active
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
            message.text = inventory[index].GetComponent<Item>().objectName + " selected";
            message.SendMessage("FadeAway");
        }
        else //deselects if selected the same button or if slot is empty
        {
            InventoryButtons[index].GetComponent<Image>().color = Color.white;
            activeSlot = -1;
        }
    }

    public void SlotDeselect()
    {
        for (int i = 0; i < 10; i++)
        {
            InventoryButtons[i].GetComponent<Image>().color = Color.white;
        }
        activeSlot = -1;
    }

    //checks if any item is selected
    public bool HasActive()
    {
        if (activeSlot == -1)
            return false;
        return true;
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
                Image[] images = InventoryButtons[i].GetComponentsInChildren<Image>();
                foreach(Image im in images)
                {
                    if(im.gameObject.GetInstanceID() != InventoryButtons[i].GetComponentInChildren<Image>().gameObject.GetInstanceID())
                    {
                        im.sprite = (item.GetComponent<Item>()).inventoryImage;
                    }
                }
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
    public void UseActive()
    {
        if(activeSlot != -1 && inventory[activeSlot] != null)
        {
            inventory[activeSlot].SetActive(true);
            Item temp = inventory[activeSlot].GetComponent<Item>();
            if (temp.weapon) //if item is weapon, using it destroys enemy
            {
                if (inRange)
                {
                    enemyInRange = true;
                    inventory[activeSlot].SendMessage("Use");
                    Debug.Log("Killed enemy"); //enemyInRange.sendMessage("Die"); ?
                    FindObjectOfType<AudioManager>().Play("EnemyDying");
                }
                else
                {
                    enemyInRange = false;
                    message.text = "No enemy in range!";
                    message.SendMessage("FadeAway");
                }
            }
            else
                inventory[activeSlot].SendMessage("Use");
            
            //if item was deactivated, usability is 0, so it is removed from inventory
            if (!inventory[activeSlot].activeInHierarchy)
            {
                Image[] images = InventoryButtons[activeSlot].GetComponentsInChildren<Image>();
                foreach (Image im in images)
                {
                    if (im.gameObject.GetInstanceID() != InventoryButtons[activeSlot].GetComponentInChildren<Image>().gameObject.GetInstanceID())
                    {
                        im.sprite = null;
                    }
                }
                inventory[activeSlot] = null;
            }
            else
            {
                inventory[activeSlot].SetActive(false);
            }

            SlotDeselect();
        }
    }

    //Drops active item from inventory on the ground
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
            drop.SendMessage("Drop"); //resets ability to be stored in inventory again
            //resets invenotory slot
            Image[] images = InventoryButtons[activeSlot].GetComponentsInChildren<Image>();
            foreach (Image im in images)
            {
                if (im.gameObject.GetInstanceID() != InventoryButtons[activeSlot].GetComponentInChildren<Image>().gameObject.GetInstanceID())
                {
                    im.sprite = null;
                }
            }
            message.text = inventory[activeSlot].GetComponent<Item>().objectName + " dropped";
            message.SendMessage("FadeAway");
            inventory[activeSlot] = null;
            SlotSelect(activeSlot);
        }
    }

    //Throws active item from inventory on the ground (with force), (similar to drop, currently unused)
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
            Image[] images = InventoryButtons[activeSlot].GetComponentsInChildren<Image>();
            foreach (Image im in images)
            {
                if (im.gameObject.GetInstanceID() != InventoryButtons[activeSlot].GetComponentInChildren<Image>().gameObject.GetInstanceID())
                {
                    im.sprite = null;
                }
            }
            inventory[activeSlot] = null;
            SlotSelect(activeSlot);
        }
    }
}
