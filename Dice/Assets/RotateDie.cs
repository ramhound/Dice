﻿using UnityEngine;
using System.Collections;

public class RotateDie : MonoBehaviour {
	private float rot;
	public bool shouldRoll;
	private Transform trans;
	public float rotationSpeed;

	// Use this for initialization
	void Start () {
		trans = transform;
	}
	
	// Update is called once per frame
	void Update () {

		if(shouldRoll) {
			float ang = transform.eulerAngles.z + (rotationSpeed * Time.deltaTime);
				 ;
			transform.rotation = Quaternion.AngleAxis(ang, Vector3.forward);
		}

	}
}
