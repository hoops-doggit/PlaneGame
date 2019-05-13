using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour {
    public bool debugRays;
    public bool viscocityRayMethod;
    [Header("Water Stats")]
    public float rayCountModifierValue;
    public float flotation;
    public float minFlotation;
    public float viscocity;
    public float depthFalloff;
    public float minHeight;
    public float width;
    public float depthToCheck;
    public float distanceBetweenDepthRays;
    public float distanceBetweenViscocityRays;
    public Vector3 actualPosition;
    public Vector3 actualVelocity;  
    
    [SerializeField]
    private float pressure;
    [SerializeField]
    private float visPressure;
    [SerializeField]
    private float depth;

    [SerializeField]
    private List<Rigidbody> rbList = new List<Rigidbody>();
    [SerializeField]
    private List<Rigidbody> rbFloatyList = new List<Rigidbody>();
    private Vector3 size;

    private BoxCollider self;

    private void Start()
    {
        self = GetComponent<BoxCollider>();
        size = self.size;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FloatyObject>())
        {
            rbFloatyList.Add(other.GetComponent<Rigidbody>());
        }
        else
        {
            rbList.Add(other.GetComponent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<FloatyObject>())
        {
            rbFloatyList.Remove(other.GetComponent<Rigidbody>());
        }
        else
        {
            rbList.Remove(other.GetComponent<Rigidbody>());
        }
    }

    private void FixedUpdate()
    {
        if (rbFloatyList.Count > 0)
        {
            for (int i = 0; i < rbFloatyList.Count; i++)
            {
                Rigidbody rb = rbFloatyList[i].GetComponent<Rigidbody>();
                FloatyObject fo = rb.GetComponent<FloatyObject>();
                Collider rbCollider = rb.GetComponent<Collider>();
                ApplyViscocityCustom(rb, fo);
                ApplyFlotationCustom(rbCollider, rb, fo);
            }
        }
        if (rbList.Count > 0)
        {
            for (int i = 0; i < rbList.Count; i++)
            {
                Rigidbody rb = rbList[i].GetComponent<Rigidbody>();
                Collider rbCollider = rb.GetComponent<Collider>();
                ApplyViscocityGeneric(rb);
                ApplyFlotationGeneric(rbCollider, rb);
            }
        }
    }   

    private void ApplyViscocityGeneric(Rigidbody rb)
    {
        if (viscocityRayMethod)
        {
            Vector3 v1 = Vector3.Cross(rb.velocity, Vector3.up);
            Vector3 v2 = Vector3.Cross(rb.velocity, v1);
            Vector3 pointOutFront = rb.GetComponent<Transform>().position + (rb.velocity.normalized * 40);

            if (debugRays)
            {
                Debug.DrawLine(pointOutFront, v1.normalized * width + pointOutFront, Color.blue);
                Debug.DrawLine(pointOutFront, -v1.normalized * width + pointOutFront, Color.blue);
                Debug.DrawLine(pointOutFront, v2.normalized * width + pointOutFront, Color.green);
                Debug.DrawLine(pointOutFront, -v2.normalized * width + pointOutFront, Color.green);
            }

            RaycastHit hit;
            for (float x = -width; x <= width; x += distanceBetweenViscocityRays)
            {
                for (float y = -width; y <= width; y += distanceBetweenViscocityRays)
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

    }

    private void ApplyViscocityCustom(Rigidbody rb, FloatyObject fo)
    {
        if (viscocityRayMethod)
        {
            Vector3 v1 = Vector3.Cross(rb.velocity, Vector3.up);
            Vector3 v2 = Vector3.Cross(rb.velocity, v1);
            Vector3 pointOutFront = rb.GetComponent<Transform>().position + (rb.velocity.normalized * 40);

            if (debugRays)
            {
                Debug.DrawLine(pointOutFront, v1.normalized * width + pointOutFront, Color.blue);
                Debug.DrawLine(pointOutFront, -v1.normalized * width + pointOutFront, Color.blue);
                Debug.DrawLine(pointOutFront, v2.normalized * width + pointOutFront, Color.green);
                Debug.DrawLine(pointOutFront, -v2.normalized * width + pointOutFront, Color.green);
            }

            RaycastHit hit;
            for (float x = -width; x <= width; x += distanceBetweenViscocityRays)
            {
                for (float y = -width; y <= width; y += distanceBetweenViscocityRays)
                {
                    Vector3 start = pointOutFront + (v1.normalized * x) + (v2.normalized * y);


                    if (Physics.Raycast(start, -rb.velocity.normalized, out hit, 40))
                    {
                        rb.AddForceAtPosition(rb.GetPointVelocity(hit.point) * -1 * viscocity * fo.waterLineOffset, hit.point, ForceMode.Force);

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
    }

    private void ApplyFlotationCustom(Collider rbCollider, Rigidbody rb, FloatyObject fo)
    {
        Vector3 pointBelow = rb.transform.position + (Vector3.down * depthToCheck);

        RaycastHit flotationHit;

        for (float x = -width * 2; x <= width * 2; x += distanceBetweenDepthRays)
        {
            for (float y = -width * 2; y <= width * 2; y += distanceBetweenDepthRays)
            {
                Vector3 start = pointBelow + (Vector3.left * x) + (Vector3.forward * y);
                if (debugRays)
                {
                    Debug.DrawLine(start, start - new Vector3(0, 10, 0));
                }

                if (Physics.Raycast(start, Vector3.up, out flotationHit, depthToCheck + width))
                {
                    if (flotationHit.point.y < gameObject.transform.position.y)
                    {
                        if (flotationHit.collider == rbCollider)
                        {
                            depth = transform.position.y - flotationHit.point.y;
                            pressure = (flotation *((depth / depthFalloff) )  + minFlotation) * fo.waterLineOffset;
                            rb.AddForceAtPosition(Vector3.up * pressure, flotationHit.point, ForceMode.Force);
                        }

                        if (debugRays)
                        {
                            Debug.DrawRay(start, Vector3.up * flotationHit.distance, Color.cyan);
                        }
                    }
                }
                else
                {
                    if (debugRays)
                    {
                        Debug.DrawRay(start, Vector3.up, Color.yellow);
                    }
                }
            }
        }
    }

    private void ApplyFlotationGeneric(Collider rbCollider, Rigidbody rb)
    {
        Vector3 pointBelow = rb.transform.position + (Vector3.down * depthToCheck);
        List<RaycastHit> raycastHits = new List<RaycastHit>();

        RaycastHit flotationHit;

        for (float x = -width * 2; x <= width * 2; x += distanceBetweenDepthRays)
        {
            for (float y = -width * 2; y <= width * 2; y += distanceBetweenDepthRays)
            {
                Vector3 start = pointBelow + (Vector3.left * x) + (Vector3.forward * y);
                if (debugRays)
                {
                    Debug.DrawLine(start, start - new Vector3(0, 10, 0));
                }

                if (Physics.Raycast(start, Vector3.up, out flotationHit, depthToCheck + width))
                {
                    if (flotationHit.point.y < gameObject.transform.position.y)
                    {
                        if (flotationHit.collider == rbCollider)
                        {
                            depth = transform.position.y - flotationHit.point.y;
                            pressure = flotation * (depth / depthFalloff) + minFlotation;
                            rb.AddForceAtPosition(Vector3.up * pressure, flotationHit.point, ForceMode.Force);
                        }

                        if (debugRays)
                        {
                            Debug.DrawRay(start, Vector3.up * flotationHit.distance, Color.cyan);
                        }
                    }
                }
                else
                {
                    if (debugRays)
                    {
                        Debug.DrawRay(start, Vector3.up, Color.yellow);
                    }
                }
            }
        }
    }

    
}
