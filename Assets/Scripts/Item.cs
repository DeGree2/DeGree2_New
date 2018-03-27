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
        //decreases usage
        if (usage > 0)
        {
            usage--;
            //deactivates item if it cannot be used anymore
            if (usage == 0)
                gameObject.SetActive(false);
            message.text = objectName + " used";
            message.SendMessage("FadeAway");
        }
        else if (usage == -1)
        {
            message.text = "Cannot use that!";
            message.SendMessage("FadeAway");
        }
    }

    public void UpdateUI()
    {
        message.text = objectName + " added to inventory";
        message.SendMessage("FadeAway");
    }
}
