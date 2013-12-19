using UnityEngine;
using System.Collections;

public class RollButton : MonoBehaviour {

	DiceDisplayer displayer;

	// Use this for initialization
	void Start () {
		displayer = transform.parent.gameObject.GetComponent<DiceDisplayer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseUpAsButton() {

        GameManager.dieValues.Clear();
        //remove selected die
		displayer.RollDice();
	}
}
