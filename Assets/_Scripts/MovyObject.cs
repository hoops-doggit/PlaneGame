﻿using UnityEngine;

public class MovyObject : MonoBehaviour {

    public float speed = 10;
    public bool engine;
    public bool startForce;


    Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        if (startForce&!engine)
        {
            rb.AddForce(Vector3.forward * speed, ForceMode.VelocityChange);
        }

	}

    private void FixedUpdate()
    {
        if (engine)
        {
            rb.AddForce(Vector3.forward * speed, ForceMode.Acceleration);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0);
        //Gizmos.DrawLine(transform.position, transform.forward*5);
    }



}
