using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour {

    public Transform t;
    public float positionLerpSpeed;
    public float rotationLerpSpeed;

	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, t.position, positionLerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, t.rotation, rotationLerpSpeed * Time.deltaTime);
    }
}
