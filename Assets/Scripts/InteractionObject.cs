using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour {
    //CHA2 & CHA3 - Milda Petrikaitė IFF-6/5

    public bool inventory; //if true, can be stored in inventory
    public bool isInInventory = false; //if true, item is currently in inventory
    public int usability = 0; //how many times item can be used, -1 for quest items

    public void DoInteraction()
    {
        //picked up and put in inventory
        gameObject.SetActive(false);
        isInInventory = true;
    }

    public void Use()
    {
        //decreases usability
        if(usability > 0)
        {
            usability--;
            //deactivates item if it cannot be used anymore
            if (usability == 0)
                gameObject.SetActive(false);
        }
        else if(usability == -1)
        {
            //special quest item usability
        }
    }
}
