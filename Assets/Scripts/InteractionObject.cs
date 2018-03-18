using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionObject : MonoBehaviour {
    public string objectName; //name of interaction object

    //general interaction method (different for item and openable object)
    public abstract void DoInteraction();
}
