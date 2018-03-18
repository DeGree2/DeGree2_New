using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : InteractionObject {
    public bool inventory = true; //if true, can be stored in inventory
    public bool isInInventory = false; //if true, item is currently in inventory
    public int usage = 1; //how many times item can be used, -1 for quest items

    public override void DoInteraction()
    {
        //picked up and put in inventory
        gameObject.SetActive(false);
        isInInventory = true;
    }

    public void Use()
    {
        //decreases usability
        if (usage > 0)
        {
            usage--;
            //deactivates item if it cannot be used anymore
            if (usage == 0)
                gameObject.SetActive(false);
        }
        else if (usage == -1)
        {
            //special quest item usability
        }
    }
}
