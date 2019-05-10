using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InstantiateBetweenTwoPoints : MonoBehaviour {

    public GameObject objectClone;
    public Transform a;
    public Transform b;
    public int number;



    public void InstantiateBetweenPoints(Transform pointA, Transform pointB, int numberOfClones)
    {
        Vector3 a = pointA.position;
        Vector3 b = pointB.position;
        Vector3 distanceBetween = (a-b) / (numberOfClones);

        for (int i = 1; i < numberOfClones; i++)
        {
            GameObject clone = Instantiate(objectClone, (distanceBetween * i) + b, Quaternion.identity);
        }
    }



	// Use this for initialization
	void Start () {
        InstantiateBetweenPoints(a, b, number);
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (GameObject x in Selection.gameObjects)
            {
                Debug.Log(x.name);
            }
        }
    }
}
