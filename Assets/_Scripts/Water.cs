using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour {

    [Header("Water Stats")]
    public float flotation;
    public float viscocity;
    
    public float depthStrength;
    [SerializeField]
    private float pressure;

    [SerializeField]
    private List<Rigidbody> rbList = new List<Rigidbody>();
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

    private Vector3 CalculateCenterpointOfRigidbody(Rigidbody rb)
    {
        //Debug.Log("Doing calsdkfja;sldfkj;aljfd");
        Transform t = rb.gameObject.transform;

        FloatyObject fo = rb.gameObject.GetComponent<FloatyObject>();
        //get this to return a vector3 at world pos

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

        
        //Instantiate(visualiser, v3List[0], Quaternion.identity, gameObject.transform);        
        //Instantiate(visualiser, v3List[1], Quaternion.identity, gameObject.transform);        
        //Instantiate(visualiser, v3List[2], Quaternion.identity, gameObject.transform);        
        //Instantiate(visualiser, v3List[3], Quaternion.identity, gameObject.transform);        
        //Instantiate(visualiser, v3List[4], Quaternion.identity, gameObject.transform);        
        //Instantiate(visualiser, v3List[5], Quaternion.identity, gameObject.transform);        
        //Instantiate(visualiser, v3List[6], Quaternion.identity, gameObject.transform);        
        //Instantiate(visualiser, v3List[7], Quaternion.identity, gameObject.transform);

        for (int i = 0; i < v3List.Count; i++)
        {
            if (self.bounds.Contains(v3List[i]))
            {
                insideme.Add(v3List[i]);
                //Instantiate(visualiser, v3List[i], Quaternion.identity, gameObject.transform);
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
        Gizmos.DrawSphere(gizPos, gizSize);
        Gizmos.color = viscocityColour;
        Gizmos.DrawCube(new Vector3(gizPos.x, gizPos.y + (viscocitySize.y/2),gizPos.z), viscocitySize);
    }

    private void OnTriggerEnter(Collider other)
    {
        rbList.Add(other.GetComponent<Rigidbody>());
    }

    private void OnTriggerExit(Collider other)
    {
        //other.GetComponent<Rigidbody>().ResetCenterOfMass();
        rbList.Remove(other.GetComponent<Rigidbody>());
    }

    private void FixedUpdate()
    {
        foreach(Rigidbody rb in rbList)
        {
            if (rb.gameObject.GetComponent<FloatyObject>())
            {
                Vector3 forcePos =  CalculateCenterpointOfRigidbody(rb);

                gizPos = forcePos;
                float depth = Mathf.Abs(transform.position.y - rb.gameObject.transform.position.y);
                pressure = flotation * (depth * depthStrength);
                Debug.Log("depth= " + depth);
                Debug.Log("pressure = " + pressure);
                //Flotation: pushes object up out of water
                rb.AddForceAtPosition(Vector3.up * pressure, forcePos, ForceMode.Acceleration);

                //rb.AddForce(Vector3.up * flotation);
                //Drag: adds force in opposite direction of travel at the level of viscocity
                //rb.AddForceAtPosition(rb.velocity *-1 * viscocity, forcePos);
                viscocitySize = rb.velocity * -1 * viscocity * 10;
                rb.AddForceAtPosition(rb.velocity * -1 * viscocity, forcePos, ForceMode.Acceleration);
                
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
