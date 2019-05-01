using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectDebug : MonoBehaviour {

    //public Transform t;
    private Rigidbody rb;
    public Vector3 v1;
    public Vector3 v2;
    public float width;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 pointOutFront = rb.transform.position + (rb.velocity.normalized * 40);
        Debug.DrawRay(rb.transform.position, rb.velocity.normalized * 40, Color.red);

        DrawCrossAtPoint(pointOutFront);

        v1 = Vector3.Cross(rb.velocity, Vector3.up);
        Debug.DrawLine(pointOutFront, v1.normalized * width + pointOutFront, Color.blue);
        Debug.DrawLine(pointOutFront, -v1.normalized * width + pointOutFront, Color.blue);

        v2 = Vector3.Cross(rb.velocity, v1);
        Debug.DrawLine(pointOutFront, v2.normalized * width + pointOutFront, Color.green);
        Debug.DrawLine(pointOutFront, -v2.normalized * width + pointOutFront, Color.green);
    }

    private void DrawCrossAtPoint(Vector3 pointOutFront)
    {
        Debug.DrawRay(pointOutFront, Vector3.up * width, Color.black);
        Debug.DrawRay(pointOutFront, Vector3.left * width, Color.black);
        Debug.DrawRay(pointOutFront, Vector3.down * width, Color.black);
        Debug.DrawRay(pointOutFront, Vector3.right * width, Color.black);
        Debug.DrawRay(pointOutFront, Vector3.forward * width, Color.black);
        Debug.DrawRay(pointOutFront, Vector3.back * width, Color.black);
    }
}
