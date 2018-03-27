using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Globalization;


public class EnemyBehaviour : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    public float timeForNewPath;
    public int attackType;
    bool inCoroutine;
    bool searchMode = false;
    NavMeshPath path;
    bool validPath;

    [Range(0, 50)]
    public float searchArea;

    bool followMode = false;

    [SerializeField]
    float attackSpeed;

    [SerializeField]
    float searchDuration;


    bool neutralMode = true;

    [SerializeField]
    string[] Points;

    bool searchBegan;

    [SerializeField]
    float speed;

    float searchModeSpeed = 100;

    [SerializeField]
    int typeOfPath;

    EnemyVision scriptVision;

    private int current = -1;
    private Vector3 target;
    private float[] X = new float[10];
    private float[] Y = new float[10];
    private float[] Z = new float[10];
    private float turnSpeed = 5f;
    private new Rigidbody rigidbody;

    bool modifiedSpeed;

    int startedSearching = 0;
    int startedSearching2 = 0;


    private bool isVisible;
    private bool playerEscaped;
    private System.Random ran = new System.Random();
    private int turn;
    private bool turning = false;
    private int turnCount = 0;

    public Transform AttackTarget;

    private bool reversePath;
    

    void GetXYZ()
    {

        int j = 0;

        foreach (string point in Points)
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


        if (current == Points.Length)
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


        if (current == Points.Length)
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
        Vector3 pos = AttackTarget.position;
        target = new Vector3(pos.x, pos.y, pos.z);

        return target;
    }

    IEnumerator Walk()
    {
        inCoroutine = true;

        if (!searchMode)
            timeForNewPath = 0;

        yield return new WaitForSeconds(timeForNewPath);
        GetNewPath();
        
        inCoroutine = false;

    }

    void GetNewPath()
    {
        if (followMode)
        {
            target = Follow();
            navMeshAgent.speed = attackSpeed;
        }
        else if (!searchMode)
        {
            navMeshAgent.speed = speed;

            switch (typeOfPath)
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
            target = getRandomPosition();
        
        navMeshAgent.SetDestination(target);
    }
    
    void Start()
    {
        GetXYZ();
        transform.LookAt(target);
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }


    void Update()
    {
        scriptVision = GetComponent<EnemyVision>();
        List<Transform> visible = scriptVision.visibleTargets;


        if (visible.Count == 0 && !searchMode && !searchBegan)
        {
            if (!inCoroutine)
                StartCoroutine(Walk());
            startedSearching = 0;
        }
        else if (visible.Count != 0)
        {
            transform.LookAt(AttackTarget);
            followMode = true;
            searchBegan = true;
            if (!inCoroutine)
                StartCoroutine(Walk());
            startedSearching = 0;
            turnCount = 0;
        }
        else if ((visible.Count == 0) && searchBegan)
        {
            searchMode = true;
            followMode = false;
            
            if (turnCount < 210)
            {
                Rotate();
                turnCount++;
            }
            else
            {
                if (!inCoroutine)
                    StartCoroutine(Walk());


                if (startedSearching >= searchDuration)
                {
                    searchBegan = false;
                    searchMode = false;
                }
                startedSearching++;
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


    }
