using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    float moveSpeed = 4f;

    Vector3 forward, right;
	void Start ()
    {
        //set axis according to MainCamera axis
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
		
	}

    // Update is called once per frame
    void Update ()
    {
        // Update for moovement
        //If player is dead, movement is disabled.
        if (HealthBarScript.health <= 0) { }

        else if (Input.anyKey)
            Move();

	}
    void Move()
    {
        // Calculate right and up movement values
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");

        //Set heading direction for our object
        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
        transform.forward = heading;

        //Transfrom (Move)
        transform.position += rightMovement;
        transform.position += upMovement;
    }

   
}
