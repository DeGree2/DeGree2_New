using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 
using UnityEditor;

[CustomEditor (typeof (EnemyVision))]
public class EnemyVisionEditor : Editor {

    private void OnSceneGUI()
    {
        EnemyVision ev = (EnemyVision)target;
        Handles.color = Color.blue;

        Handles.DrawWireArc(ev.transform.position, Vector3.up, Vector3.forward, 360, ev.viewRadius);
        Vector3 viewAngleA = ev.DirFromAngle(-ev.viewAngle / 2, false);
        Vector3 viewAngleB = ev.DirFromAngle(ev.viewAngle / 2, false);

        Handles.DrawLine(ev.transform.position, ev.transform.position + viewAngleA * ev.viewRadius);
        Handles.DrawLine(ev.transform.position, ev.transform.position + viewAngleB * ev.viewRadius);

        Handles.color = Color.red;

        foreach (Transform visibleTarget in ev.visibleTargets)
        {
            Handles.DrawLine(ev.transform.position, visibleTarget.position);


        }
    }

}

#endif