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
    public Transform sparkle;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        behavior = GetComponentInParent<EnemyBehaviour>();
        vision = GetComponentInParent<EnemyVision>();
        sparkle.GetComponent<ParticleSystem>().enableEmission = false;
    }


    // Update is called once per frame
    void Update()
    {
        posForw = new Vector3(behavior.playerTarget.transform.position.x, vision.position.y, behavior.playerTarget.transform.position.z);
        pos = vision.position;

        if (behavior.damaged)
        {
            lr.enabled = false;
            sparkle.GetComponent<ParticleSystem>().enableEmission = false;
        }
        else if (behavior.withLaser && behavior.useLaser)
        {
            DrawLaser();
            lr.enabled = true;
        }
        else
        {
            sparkle.GetComponent<ParticleSystem>().enableEmission = false;
            lr.enabled = false;
        }
    }

    void DrawLaser()
    {
        transform.position = pos;
        lr.SetPosition(0, transform.position);

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                sparkle.GetComponent<ParticleSystem>().enableEmission = true;
                GameObject laserPoint = GameObject.FindWithTag("laser_Point");

                if (behavior.visibleT.Count != 0)
                {
                    Vector3 posi = new Vector3(hit.point.x, laserPoint.transform.position.y, hit.point.z);
                    lr.SetPosition(1, posi);
                    sparkle.transform.position = posi;
                }
                else
                {
                    lr.SetPosition(1, hit.point);
                    sparkle.transform.position = hit.point;
                }
            }

        }
        else lr.SetPosition(1, transform.forward * 10);
    }
}