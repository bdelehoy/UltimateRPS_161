using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour {


    public GameObject playerTarget;

    [SerializeField]
    private float xMax;
    [SerializeField]
    private float yMax;
    [SerializeField]
    private float xMin;
    [SerializeField]
    private float yMin;

    private Transform target;

	// Use this for initialization
	void Start () {
        target = playerTarget.transform; 
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//transform.position = new Vector3(Mathf.Clamp(target.position.x + 1,xMin,xMax),Mathf.Clamp(target.position.y, yMin, yMax),-10);
	}
}
