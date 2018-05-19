using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour {
    public Camera cam;
    private Vector3 camCoords;
    private Vector3 camCoordsEnd;
    public GameObject player;
    private PlayerInteract inter;
    public GameObject canvas1;
    public GameObject canvas2;
    public Light changingLight;
    private Color red;
    private Color normal;
    private bool changing = false;
    private bool counting = false;
    private bool counted = false;
    private float step;
    public GameObject levelCompleted;
    public GameObject PauseMenu;

    // Use this for initialization
    void Start () {
        inter = player.GetComponent<PlayerInteract>();
        camCoords = new Vector3(0,0,0);
        camCoordsEnd = new Vector3(25, 6, 38);
        normal = changingLight.color;
        red = new Color(1, 0.25f, 0.25f, 1);
        step = Vector3.Distance(camCoords, camCoordsEnd) / 200f;
    }
	
	// Update is called once per frame
	void Update () {
        if (!changing)
        {
            StartCoroutine("ColorChange");
        }
        if (inter.enemyDeath)
        {
            if (!counting && !counted)
            {
                StartCoroutine("Counting");
            }
            if (counted)
            {
                if (camCoords.x == 0 && camCoords.y == 0 && camCoords.z == 0)
                {
                    camCoords = cam.transform.position;
                    cam.GetComponentInParent<CameraFollow>().enabled = false;
                    canvas1.SetActive(false);
                    canvas2.SetActive(false);
                }
                if (camCoords != camCoordsEnd)
                {
                    camCoords = Vector3.MoveTowards(camCoords, camCoordsEnd, step);
                    if(cam.orthographicSize <= 15)
                        cam.orthographicSize += 0.05f;
                }
                HealthBarScript.health = 100;
                cam.transform.position = camCoords;
            }
        }
	}

    private IEnumerator ColorChange()
    {
        changing = true;
        FindObjectOfType<AudioManager>().Play("alarm");
        changingLight.color = red;
        yield return new WaitForSecondsRealtime(0.75f);
        changingLight.color = normal;
        yield return new WaitForSecondsRealtime(3f);
        changing = false;
    }

    private IEnumerator Counting()
    {
        counting = true;
        PauseMenu.SendMessage("CannotPause");
        if (Time.timeScale != 0)
        {
            FindObjectOfType<AudioManager>().Play("shutDownIniciated");
            yield return new WaitForSecondsRealtime(2f);
        }
        if (Time.timeScale != 0)
        {
            FindObjectOfType<AudioManager>().Play("robotNoise");
            yield return new WaitForSecondsRealtime(1f);
        }
        if(Time.timeScale != 0)
        {
            FindObjectOfType<AudioManager>().Play("counting");
            yield return new WaitForSecondsRealtime(10f);
            if (Time.timeScale != 0)
            {
                counted = true;
            }
                
        }
        if (Time.timeScale != 0) //player is immortal at this point
        {
            yield return new WaitForSecondsRealtime(1f);
            FindObjectOfType<AudioManager>().Play("EnemyDying");
            FindObjectOfType<AudioManager>().Play("MassShutDown");
            FindObjectOfType<AudioManager>().Play("RobotDying2");
            yield return new WaitForSecondsRealtime(0.1f);
            FindObjectOfType<AudioManager>().Play("EnemyDying");
            yield return new WaitForSecondsRealtime(3f);
            levelCompleted.SetActive(true);
            levelCompleted.transform.position = player.transform.position;
            counting = false;
        }
    }

}
