using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Globalization;


public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    int pathType;

    [SerializeField]
    string[] WayPoints;

    [SerializeField]
    float speed;

    [SerializeField]
    float attackSpeed;

    [SerializeField]
    public bool withLaser;

    [SerializeField]
    float searchDuration;

    [SerializeField]
    Inventory playerDamager;
    public Transform playerTarget;

    [HideInInspector]
    public bool damaged;

    int count;
    Vector3 curPosition;

    int damageRange;
    bool doDamage;
    float damageSpeed = 2.0f;

    bool notFirst = false;

    [HideInInspector]
    public bool changeLayerToDefault;
    bool isInvestigating;
    Vector3 whereFell;
    int goLookDuration;
    int investigateDuration;
    float distFromObj;
    bool firstInvestig = true;

    float distFromPlayer;
    float dist;

    int collideAgain;

    NavMeshAgent navMeshAgent;
    int narrow = 0;
    bool inCoroutine;
    bool searchMode = false;
    NavMeshPath path;
    bool searchBegan;
    float searchModeSpeed = 100;

    bool beginLaser;
    bool endLaser;
    int endL;
    [HideInInspector]
    public bool useLaser;

    int count2;
    int firstTime0 = 0;
    bool first;

    Vector3 target2;
    Vector3 target3;

    [HideInInspector]
    public List<Transform> visibleT;

    private float timestamp = 0.0f;

    bool wasInSearch;
    bool isAtWall;
    int timeAtWall = 0;

    int firstCor = 0;

    float searchArea = 50;
    bool followMode = false;
    int timeForNewPath = 0;

    EnemyVision scriptVision;

    int current = -1;
    Vector3 target;

    float[] X = new float[10];
    float[] Y = new float[10];
    float[] Z = new float[10];
    Animator anim;

    int startedSearching = 0;

    System.Random ran = new System.Random();
    int turn;
    bool turning = false;
    int turnCount = 0;
    int investigEnd;

    bool reversePath;
    float distance;


    void GetXYZ()
    {

        int j = 0;

        foreach (string point in WayPoints)
        {
            string[] values = point.Split(';');

            X[j] = float.Parse(values[0], CultureInfo.InvariantCulture);
            Y[j] = float.Parse(values[1], CultureInfo.InvariantCulture);
            Z[j] = float.Parse(values[2], CultureInfo.InvariantCulture);
            j++;

        }

    }

    Vector3 getRandomPosition()
    {
        float x = Random.Range(-searchArea, searchArea);
        float z = Random.Range(-searchArea, searchArea);

        Vector3 pos = new Vector3(x, 0, z);
        return pos;
    }

    Vector3 getSetPosition1()
    {

        float dist = navMeshAgent.remainingDistance;


        if (current == WayPoints.Length)
        {
            current = 0;
            target = new Vector3(X[current], Y[current], Z[current]);

        }
        else if (dist != Mathf.Infinity && navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && navMeshAgent.remainingDistance == 0)
        {
            current++;
            target = new Vector3(X[current], Y[current], Z[current]);

        }

        return target;
    }


    Vector3 getSetPosition2()
    {


        float dist = navMeshAgent.remainingDistance;


        if (current == WayPoints.Length)
        {
            reversePath = true;
            current--;
            target = new Vector3(X[current], Y[current], Z[current]);

        }
        else if (dist != Mathf.Infinity && navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && navMeshAgent.remainingDistance == 0)
        {
            if (!reversePath)
            {
                current++;
                target = new Vector3(X[current], Y[current], Z[current]);
            }
            else
            {
                current--;
                target = new Vector3(X[current], Y[current], Z[current]);
            }
        }
        if (current == 0 && reversePath)
        {
            reversePath = false;
            target = new Vector3(X[current], Y[current], Z[current]);
        }

        return target;
    }


    Vector3 Follow()
    {
        Vector3 pos = playerTarget.position;
        target = new Vector3(pos.x, pos.y, pos.z);

        return target;
    }

    IEnumerator Walk()
    {
        yield return new WaitForSeconds(0);
        GetNewPath();

        inCoroutine = false;

    }

    void GetNewPath()
    {
        if (!searchMode)
        {
            navMeshAgent.speed = speed;
            //navMeshAgent.speed = 0;

            switch (pathType)
            {
                case 0:
                    target = getSetPosition1();
                    break;
                case 1:
                    target = getSetPosition2();
                    break;
            }

        }
        else if (searchMode)
        {

            if (!first && timeAtWall > 10)
            {
                target = getRandomPosition();
                timeAtWall = 0;
            }

            if (timeForNewPath > 10)
            {
                first = false;
            }

            if (timeForNewPath >= 70)
            {
                target = getRandomPosition();
                timeForNewPath = 0;
            }
            timeForNewPath++;

            if (isAtWall)
                timeAtWall++;


        }

        navMeshAgent.SetDestination(target);
    }

    void Start()
    {
        GetXYZ();
        transform.LookAt(target);
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        anim = GetComponent<Animator>();
        damageRange = playerDamager.damageRange;
    }


    void Update()
    {
        scriptVision = GetComponent<EnemyVision>();
        visibleT = scriptVision.visibleTargets;
        List<Transform> visibleO = scriptVision.visibleObjects;

        dist = Vector3.Distance(playerTarget.position, transform.position);

        //transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (visibleO.Count != 0)
        {
            whereFell = visibleO[0].position;
            distFromObj = Vector3.Distance(whereFell, transform.position);
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        }

        distFromPlayer = Vector3.Distance(playerDamager.transform.position, transform.position);

        if (transform.tag == "enemy" && playerDamager.enemyInRange && distFromPlayer <= damageRange)
        {
            damaged = true;
            transform.tag = "Untagged";


            if (playerDamager.damageOnly1)
                playerDamager.enemyInRange = false;
        }


        if ((transform.tag == "Untagged") && damaged)
        {
            
            navMeshAgent.speed = 0;

            if (narrow >= 3 && scriptVision.viewAngle != 0)
            {
                scriptVision.viewAngle = scriptVision.viewAngle - 1;
                narrow = 0;
            }
            narrow++;
            //stand-in-place or death animation                                           //ANIMATION
			anim.SetTrigger("isDead");

            if (withLaser && endLaser)
            {
                FindObjectOfType<AudioManager>().Play("Laser2");
                endLaser = false;
            }
            //makes body unmovable when enemy is dead; some errors are shown because of this, but it works
            gameObject.GetComponent<NavMeshAgent>().enabled = false;

        }
        else if (visibleT.Count == 0 && visibleO.Count != 0)
        {
            changeLayerToDefault = false;

            Vector3 visib = new Vector3(visibleO[0].position.x, 0, visibleO[0].position.z);
            transform.LookAt(visib);

            if (goLookDuration < 50)
            {
                navMeshAgent.speed = 0;
                distance = distFromObj;
                count2 = 0;
                //stand-in-place animation                                                //ANIMATION
            }
            else if (investigateDuration < 200 && goLookDuration > 50 && distFromObj > 3)
            {
                
                navMeshAgent.speed = speed;
                navMeshAgent.SetDestination(visib);
                
                if (count2 > 50)
                {
                    if (Mathf.Abs(distFromObj - distance) < 1)
                    {
                        investigateDuration = 200;
                    }
                    else
                    {
                        distance = distFromObj;
                        count2 = 0;
                    }
                }

                count2++;
            }
            else if (distFromObj <= 3 && investigateDuration < 200)
            {
                navMeshAgent.speed = 0;
                investigateDuration++;
                //stand-in-place animation                                                //ANIMATION
            }
            else if (investigateDuration == 200)
            {
                goLookDuration = 0;
                investigateDuration = 0;
                navMeshAgent.speed = speed;
                changeLayerToDefault = true;
            }

            goLookDuration++;

        }
        else
        {

            if (transform.hasChanged)
            {
                isAtWall = false;
                transform.hasChanged = false;

            }
            else
            {
                isAtWall = true;
            }

            
            //neutral
            if (visibleT.Count == 0 && !searchMode && !searchBegan)
            {

                if (isAtWall)
                    timeAtWall++;

                target3 = target;

                if (!inCoroutine)
                    StartCoroutine(Walk());



                if (target == target2 && wasInSearch)
                {
                    target = new Vector3(X[current], Y[current], Z[current]);
                    wasInSearch = false;
                }


                if (target == target3 && timeAtWall >= 20)
                {
                    timeAtWall = 0;
                    navMeshAgent.speed = 0;
                    transform.LookAt(-target);
                }


                startedSearching = 0;
                anim.SetBool("isAttacking", false);
                anim.SetBool("isLooking", false);

                beginLaser = true;
            }
            //attack
            else if (visibleT.Count != 0)
            {
                gameObject.GetComponent<NavMeshAgent>().isStopped = false;
                wasInSearch = false;
                transform.LookAt(playerTarget);
                followMode = true;
                searchBegan = true;
                target = Follow();
                navMeshAgent.SetDestination(target);

                startedSearching = 0;
                turnCount = 0;

                firstCor = 0;
                timeAtWall = 0;
                first = true;

                if (withLaser)
                {
                    if (beginLaser)
                    {
                        useLaser = true;
                        FindObjectOfType<AudioManager>().Play("Laser1");
                        beginLaser = false;
                        endLaser = true;
                        endL = 0;
                    }

                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isLooking", false);
                    navMeshAgent.speed = speed;
                    InvokeRepeating("Damage", 0, 0.5f);

                    if (dist <= 5)
                    {
                        navMeshAgent.speed = 0;
                    }
                }
                else
                {
                    anim.SetBool("isAttacking", true);
                    anim.SetBool("isLooking", false);
                    navMeshAgent.speed = attackSpeed;

                    if (dist <= 2 && doDamage)
                    {
                        navMeshAgent.speed = 0;
                        InvokeRepeating("Damage", 0, 0.5f);
                    }
                    else CancelInvoke("Damage");

                }



            }
            //search
            else if ((visibleT.Count == 0) && searchBegan)
            {
                navMeshAgent.speed = attackSpeed;
                firstTime0 = 0;
                CancelInvoke("Damage");


                if (withLaser && endLaser)
                {
                    FindObjectOfType<AudioManager>().Play("Laser2");
                    endLaser = false;
                }


                if (withLaser && endL >= 65)
                {
                    useLaser = false;
                }
                else endL++;


                searchMode = true;
                followMode = false;
                wasInSearch = true;
                beginLaser = true;

                if (navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial && firstCor <= 50)
                {
                    anim.SetBool("isAttacking", true);
                    anim.SetBool("isLooking", false);

                }
                else if (firstCor > 50 && turnCount < 210)
                {
                    gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isLooking", true);
                    Rotate();
                    turnCount++;
                }
                else if (firstCor < 50)
                {
                    anim.SetBool("isAttacking", true);
                    anim.SetBool("isLooking", false);
                }
                else
                {
                    gameObject.GetComponent<NavMeshAgent>().isStopped = false;
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isLooking", false);
                    if (!inCoroutine)
                        StartCoroutine(Walk());


                    if (startedSearching >= searchDuration)
                    {
                        searchBegan = false;
                        searchMode = false;

                    }
                    startedSearching++;
                }

                firstCor++;

                target2 = target;

            }
        }
    }


    public void Rotate()
    {
        if (!turning)
        {
            turn = ran.Next(0, 2);
            turning = true;
        }
        Vector3 angles;
        angles = transform.rotation.eulerAngles;
        switch (turn)
        {

            case 0:
                angles.y -= Time.deltaTime * searchModeSpeed;

                break;
            case 1:
                angles.y += Time.deltaTime * searchModeSpeed;
                break;
        }
        transform.rotation = Quaternion.Euler(angles);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doDamage = true;
        }
        else doDamage = false;
    }

    private void Damage()
    {
        if (!damaged && HealthBarScript.health > 0)
        {
            if (Time.time >= timestamp)
            {
                HealthBarScript.Damage();
                timestamp = Time.time + damageSpeed;
            }
        }

    }

}
