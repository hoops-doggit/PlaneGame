using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boyancy : MonoBehaviour {

    public float flotation;
    public float speed;
    private Rigidbody rb;
    public bool inWater;
    public float waterViscosity;
    

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Water")
        {
            inWater = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            inWater = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {
            inWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {
            inWater = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (inWater)
        {
            //Boyancy: adds force up.
            //Could possibly be opposite to gravity?
            rb.AddForce(Vector3.up * flotation);
            //Drag: adds force in opposite direction of travel
            rb.AddForce(rb.velocity * -1 * waterViscosity);
        }


        if (!inWater)
        {
            //Speed: the force being applied by engines
            //rb.AddForce(Vector3.forward * speed);
        }

	}


}
