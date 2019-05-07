using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyCustomPivot : MonoBehaviour {

    public Transform t;
    public Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        
	}

    private void FixedUpdate()
    {
       // rb.centerOfMass = t.position;
    }


}
