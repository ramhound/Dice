﻿using UnityEngine;
using System.Collections;

public class RotateDie : MonoBehaviour {
	private float rot;
	private bool shouldRoll;
	private Transform trans;
	public float rotationSpeed;

	// Use this for initialization
	void Start () {
		trans = transform;
	}
	
	// Update is called once per frame
	void Update () {

		if(shouldRoll) {
            float ang = trans.eulerAngles.z + (rotationSpeed * Time.deltaTime);
            trans.rotation = Quaternion.AngleAxis(ang, Vector3.forward);
		}
	}

    public void StartRotation() {
        shouldRoll = true;
    }

    public void StopRotation() {
        shouldRoll = false;
    }
}
