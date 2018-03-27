﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private float speed;
    private float run_speed = 1.0f;

    Animator anim;

	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isRunning", true);
            anim.SetBool("isIdle", false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("isRunning", true);
            anim.SetBool("isIdle", false);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("isRunning", true);
            anim.SetBool("isIdle", false);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isRunning", true);
            anim.SetBool("isIdle", false);
        }
        else
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isIdle", true);
        }
	}
}