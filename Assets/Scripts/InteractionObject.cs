using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractionObject : MonoBehaviour {
    public string objectName; //name of interaction object
    public Text message; //text of message associated with interactions

    //general interaction method (different for item and openable object)
    public abstract void DoInteraction();
}
