using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTalking : MonoBehaviour {
    public string sound;
    private bool talking = false;
    private EnemyBehaviour en;
    public GameObject player;

    private void Start()
    {
        en = GetComponent<EnemyBehaviour>();
    }

    private void Update()
    {
        if (!en.damaged)
        {
            if (!talking)
            {
                StartCoroutine("Talking");
            }
        }
        
    }

    private IEnumerator Talking()
    {
        talking = true;
        if (Time.timeScale != 0)
        {
            int num = Random.Range(10, 60);
            yield return new WaitForSecondsRealtime(num);
            if (Time.timeScale != 0 && !en.damaged && Vector3.Distance(player.gameObject.transform.position, gameObject.transform.position) < 15)
            {
                talking = false;
                FindObjectOfType<AudioManager>().Play(sound);
            }
            else
            {
                talking = false;
            }
                
        }
        else
        {
            talking = false;
            yield return null;
        }
    }
}
