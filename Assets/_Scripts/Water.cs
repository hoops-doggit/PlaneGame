using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour {

    [Header("Water Stats")]
    public float flotation;
    public float viscocity;
    public float depthStrength;
    public float minHeight;
    public float depthMinValue;
    public float width;
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
    private List<FloatyObject> foList = new List<FloatyObject>();
    [SerializeField]
    private List<Transform> tList = new List<Transform>();
    [SerializeField]
    private List<Vector3> v3List = new List<Vector3>();
    [SerializeField]
    private List<Vector3> insideme = new List<Vector3>();
    private Vector3 size;

    public float x;
    public float y;
    public float z;
    public float insideMeLength;


    public GameObject visualiser;
    public float visualiserSize;

    private BoxCollider self;

    [Header("For Drawing Gizmos")]
    public Color currentCenterPointCol = new Color(0, 0, 1, 0.5f);
    private Vector3 gizPos = new Vector3(0, 0, 0);
    public float gizSize;
    public Color viscocityColour;
    public Vector3 viscocitySize;

    private void Start()
    {
        self = GetComponent<BoxCollider>();
        size = self.size;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FloatyObject>())
        {
            rbList.Add(other.GetComponent<Rigidbody>());
            foList.Add(other.GetComponent<FloatyObject>());
            tList.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //other.GetComponent<Rigidbody>().ResetCenterOfMass();

        if(other.GetComponent<FloatyObject>())
        {
            rbList.Remove(other.GetComponent<Rigidbody>());
            foList.Remove(other.GetComponent<FloatyObject>());
            tList.Remove(other.transform);
        }
    }

    private Vector3 CalculateCenterpointOfRigidbody(int zx)
    {
        //Debug.Log("Doing calsdkfja;sldfkj;aljfd");

        //get this to return a vector3 at world pos

        Rigidbody rb = rbList[zx];
        FloatyObject fo = foList[zx];
        Transform t = tList[zx];

        //add all x values
        v3List.Clear();
        insideme.Clear();

        x = 0;
        y = 0;
        z = 0;
        insideMeLength = 0;

        for(int i = 0; i < fo.points.Count; i++)
        {
            v3List.Add(fo.points[i].position);
        }
        for (int i = 0; i < v3List.Count; i++)
        {
            if (self.bounds.Contains(v3List[i]))
            {
                insideme.Add(v3List[i]);
                //instantiates a shere 
                //GameObject vis = Instantiate(visualiser, v3List[i], Quaternion.identity, gameObject.transform);
                //vis.transform.SetParent(null);
                //vis.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        insideMeLength = insideme.Count;

        //get length of list
        foreach (Vector3 v3 in insideme)
        {
            x += v3.x;
            y += v3.y;
            z += v3.z;
        }
        Vector3 result = new Vector3(x / insideMeLength, y / insideMeLength, z / insideMeLength);
        //GameObject clone = Instantiate(visualiser, result, Quaternion.identity, gameObject.transform);
        //clone.transform.parent = null;
        //clone.transform.localScale = Vector3.one * visualiserSize;
        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = currentCenterPointCol;
        //Gizmos.DrawSphere(gizPos, gizSize);
        Gizmos.color = viscocityColour;
        //Gizmos.DrawCube(new Vector3(gizPos.x, gizPos.y + (viscocitySize.y/2),gizPos.z), viscocitySize);
        //Gizmos.DrawLine(new Vector3(gizPos.x, gizPos.y, gizPos.z), viscocitySize);        
    }



    private void FixedUpdate()
    {
        for(int i = 0; i < rbList.Count; i++)
        {
            Vector3 underWaterCenterpoint =  CalculateCenterpointOfRigidbody(i);
            gizPos = underWaterCenterpoint;
            actualPosition = underWaterCenterpoint;
            //depth = transform.position.y - tList[i].position.y;
            depth = Mathf.Abs( transform.position.y - underWaterCenterpoint.y);

            //old pressure
            pressure = flotation * (depth / depthStrength);

            //too simple new pressure
            //if(underWaterCenterpoint.y > -5)
            //{
            //    pressure = flotation / 2f;
            //}
            //else
            //{
            //    pressure = flotation;
            //}
            Vector3 v1 = Vector3.Cross(rbList[i].velocity, Vector3.up);
            Vector3 v2 = Vector3.Cross(rbList[i].velocity, v1);
            Vector3 pointOutFront = rbList[i].GetComponent<Transform>().position + (rbList[i].velocity.normalized * 40);
            RaycastHit hit;
            for (float x = -width; x < width; x += 1)
            {
                for (float y = -width; y < width; y += 1)
                {
                    Vector3 start = pointOutFront + (v1 * x) + (v2 * y);
                    if (Physics.Raycast(start, -rbList[i].GetComponent<Transform>().position, out hit, 40))
                    {
                        Debug.DrawRay(start, -rbList[i].velocity.normalized * hit.distance, Color.red);

                    }
                    else
                    {
                        Debug.DrawRay(start, -rbList[i].velocity.normalized * hit.distance, Color.yellow);
                    }
                    
                }
            }

            visPressure = viscocity * (depth / depthStrength);
            //debug.Logs in fixed update kills performance
            //Debug.Log("depth= " + depth);
            //Debug.Log("pressure = " + pressure);

            //Flotation: pushes object up out of water
            //rb.AddForce(Vector3.up * flotation);
            rbList[i].AddForceAtPosition(Vector3.up * pressure, underWaterCenterpoint, ForceMode.Impulse);

            
            

            //Drag: adds force in opposite direction of travel at the level of viscocity
            //rb.AddForceAtPosition(rb.velocity *-1 * viscocity, forcePos);

            rbList[i].AddForceAtPosition(rbList[i].GetPointVelocity(underWaterCenterpoint) * -1 * viscocity, underWaterCenterpoint, ForceMode.Impulse);
            //rbList[i].AddTorque(rbList[i].angularVelocity * -1 * viscocity, underWaterCenterpoint, ForceMode.Impulse);
            actualVelocity= rbList[i].velocity;

            //For Debugging
            //viscocitySize = rbList[i].velocity * -1 * visPressure * 10;
        }
    }
}
