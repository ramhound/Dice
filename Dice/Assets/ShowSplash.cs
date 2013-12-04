using UnityEngine;
using System.Collections;

public class ShowSplash : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SplashScreen splash = SOS.CreateObjectAt<SplashScreen>("Splash Screen", Vector2.zero);
		splash.onSplashFinished = splashFinished;
	}
	
	// Update is called once per frame
	void Update () {
	

	}

	public void splashFinished () {
		//print ("Test");
	}
}
