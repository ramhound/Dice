using UnityEngine;
using System.Collections;

public class StarMovement : MonoBehaviour {

	Transform trans;
	public Vector2 speed;

	// Use this for initialization
	void Start () {
		trans = transform;
		iTween.MoveTo(gameObject, iTween.Hash("position", Vector3.zero, "time", 3.0f, "easetype", iTween.EaseType.easeOutQuart));
	}
	
	// Update is called once per frame
	void Update () {
		//trans.position = trans.position.Add(speed * Time.deltaTime);
	}
}
