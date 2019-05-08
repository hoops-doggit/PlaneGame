using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck_Controls : MonoBehaviour {

    public bool debugRays;
    public GameObject debugpos;
    private Rigidbody rb;
    public float maxVelocity;
    public float speed;
    public float brakingSpeed;
    public float turnBreakSpeed;
    public float rotationSpeed;
    public float backSpeed;
    public GameObject frontCollider;
    public Vector3 angularVelocity;
    public float viscocity;
    public float width;
    public float distanceBetweenViscocityRays;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}

    private void ApplyViscocity()
    {
        //Vector3 underWaterCenterpoint = CalculateCenterpointOfRigidbody(i);
        ///start raycasting
        ///

        Vector3 v1 = Vector3.Cross(rb.velocity, Vector3.up);
        Vector3 v2 = Vector3.Cross(rb.velocity, v1);
        Vector3 pointOutFront = rb.GetComponent<Transform>().position + (rb.velocity.normalized * 40);
        //debugpos.transform.position = pointOutFront;

        if (debugRays)
        {
            Debug.DrawLine(pointOutFront, v1.normalized * width + pointOutFront, Color.blue);
            Debug.DrawLine(pointOutFront, -v1.normalized * width + pointOutFront, Color.blue);
            Debug.DrawLine(pointOutFront, v2.normalized * width + pointOutFront, Color.green);
            Debug.DrawLine(pointOutFront, -v2.normalized * width + pointOutFront, Color.green);
        }

        RaycastHit hit;
        for (float x = -width; x < width; x += distanceBetweenViscocityRays)
        {
            for (float y = -width; y < width; y += distanceBetweenViscocityRays)
            {
                Vector3 start = pointOutFront + (v1.normalized * x) + (v2.normalized * y);


                if (Physics.Raycast(start, -rb.velocity.normalized, out hit, 40))
                {
                    rb.AddForceAtPosition(rb.GetPointVelocity(hit.point) * -1 * viscocity, hit.point, ForceMode.Force);


                    if (debugRays)
                    {
                        Debug.DrawRay(start, -rb.velocity.normalized * hit.distance, Color.red);
                    }

                }
                else
                {
                    if (debugRays)
                    {
                        Debug.DrawRay(start, -rb.velocity.normalized, Color.yellow);
                    }
                }
            }
        }
    }

        // Update is called once per frame
    void Update () {


        




        angularVelocity = rb.angularVelocity;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(rb.transform.forward * speed);
            frontCollider.SetActive(true);
        }
        if(!Input.GetKey(KeyCode.UpArrow)) {
            frontCollider.SetActive(false);
            rb.AddForce(rb.velocity * -1* brakingSpeed);
            //ApplyViscocity();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddRelativeTorque(Vector3.up * -rotationSpeed);
        }
        if(!Input.GetKey(KeyCode.LeftArrow) )
        {            
            rb.AddTorque(rb.angularVelocity *-1*turnBreakSpeed);
            //ApplyViscocity();
        }
        if (!Input.GetKey(KeyCode.RightArrow))
        {

            rb.AddTorque(rb.angularVelocity *-1*turnBreakSpeed);
            //ApplyViscocity();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddRelativeTorque(Vector3.up * rotationSpeed);
        }
        else
        {
            //if (rb.angularVelocity.z > 0)
              //  rb.AddRelativeTorque(-rotationSpeed * Vector3.up);
        }

        if (Input.GetKey(KeyCode.DownArrow)){
            rb.AddRelativeTorque(rb.transform.right * backSpeed);
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);


        //rb.velocity.Set(Mathf.Lerp(0, ForceMode.Force);
        
	}
}
