using UnityEngine;
using System.Collections;

public class EnemyLaser : MonoBehaviour
{
    RaycastHit hit;
    EnemyBehaviour behavior;
    EnemyVision vision;
    private LineRenderer lr;
    Vector3 pos;
    Vector3 posForw;


    void Start()
    {
        lr = GetComponent<LineRenderer>();
        behavior = GetComponentInParent<EnemyBehaviour>();
        vision = GetComponentInParent<EnemyVision>();
    }


    // Update is called once per frame
    void Update()
    {
        posForw = new Vector3(behavior.playerTarget.transform.position.x, vision.position.y, behavior.playerTarget.transform.position.z);
        pos = vision.position;

        if (behavior.damaged)
        {
            lr.enabled = false;
        }
        else if (behavior.withLaser && behavior.useLaser)
        {
            DrawLaser();
            lr.enabled = true;
        }
        else lr.enabled = false;
    }

    void DrawLaser()
    {
        transform.position = pos;
        lr.SetPosition(0, transform.position);

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                if (behavior.visibleT.Count != 0)
                    lr.SetPosition(1, posForw);
                else lr.SetPosition(1, hit.point);
            }
        }
        else lr.SetPosition(1, transform.forward * 10);
    }
}