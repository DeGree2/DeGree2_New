using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScenes : MonoBehaviour {

    [SerializeField] private string loadLevel;
    public bool change = false;
    public float delayTime;

    private void Update()
    {
        if (change)
            StartCoroutine("Timer");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            change = true;
        }
    }

    IEnumerator Timer()
    {
       // HealthBarScript.levelCompleted = true;
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(loadLevel);
        StopCoroutine("Timer");
    }
}
