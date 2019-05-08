using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck_Controls : MonoBehaviour {

    private Rigidbody rb;
    public float speed;
    public float brakingSpeed;
    public float rotationSpeed;
    public float backSpeed;
    public GameObject frontCollider;
    public Vector3 angularVelocity;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        angularVelocity = rb.angularVelocity;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(rb.transform.forward * speed);
            frontCollider.SetActive(true);
        }
        else {
            frontCollider.SetActive(false);
            rb.AddForce(-rb.velocity * brakingSpeed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddRelativeTorque(Vector3.up * -rotationSpeed);
        }
        if(!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            
            rb.AddRelativeTorque(-rb.angularVelocity * brakingSpeed);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddRelativeTorque(Vector3.up * rotationSpeed);
        }
        else
        {
            if (rb.angularVelocity.y < 0)
                rb.AddRelativeTorque(-rotationSpeed * Vector3.up);
        }

        if (Input.GetKey(KeyCode.DownArrow)){
            rb.AddRelativeTorque(rb.transform.right * backSpeed);
        }
        
	}
}
