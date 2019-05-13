using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyObject : MonoBehaviour {

    public List<Transform> points;
    public float waterLineOffset = 1;
    public float rayProjectionWidth = 20;
    public float widthBetweenDepthRays = 10;

    private void Start()
    {
        float mass  = gameObject.GetComponent<Rigidbody>().mass;
        if(mass <= 1)
        {
            waterLineOffset = mass;
        } 
    }

}
