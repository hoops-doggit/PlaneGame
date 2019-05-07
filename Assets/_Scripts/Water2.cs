using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water2 : MonoBehaviour {

    public int waterWidth;
    public int distanceBetweenOverallRays;
    public float rayLength;
    public float rayDepth;
    private Vector3 underwaterRayStartPoint;


	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        
        RaycastHit underWaterHit;
        for (float x = -waterWidth * 2; x < waterWidth * 2; x += distanceBetweenOverallRays)
        {
            for (float y = -waterWidth * 2; y < waterWidth * 2; y += distanceBetweenOverallRays)
            {
                underwaterRayStartPoint = new Vector3(x,transform.position.y,y);
                if (Physics.Raycast(underwaterRayStartPoint, Vector3.down * rayLength, out underWaterHit, 0))
                {
                    Debug.DrawRay(new Vector3(x, rayDepth, y), Vector3.down * rayLength, Color.red);
                }

                else
                {
                    Debug.DrawRay(new Vector3(x, rayDepth, y), Vector3.down * rayLength, Color.cyan);
                }
                
            }
        }
    }
}
