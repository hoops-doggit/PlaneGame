using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckFlipper : MonoBehaviour {

    public GameObject emergencyFlipper;
    public float yDotProduct;
    private Rigidbody rb;
    public float torque;
    private Vector3 angleToRotateOn;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {


        yDotProduct = Vector3.Dot(rb.transform.up, Vector3.up);
        angleToRotateOn = Vector3.Cross(transform.forward, Vector3.up);

        if(yDotProduct > -0.8f)
        {
            //rb.AddTorque(Vector3.right * torque, ForceMode.Force);
            rb.AddRelativeTorque(angleToRotateOn * torque, ForceMode.Force);
            emergencyFlipper.SetActive(true);
        }

        else
        {
            emergencyFlipper.SetActive(false);
        }
	}
}
