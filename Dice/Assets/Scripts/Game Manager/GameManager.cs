using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance { get; private set; }

    void Awake() { instance = this; }

	// Use this for initialization
	void Start () {
        //GameObject[] dice = gameObject.FindChildrenByBeginningOfNameRecursively("Die");
        //foreach(GameObject go in dice)
        //    go.SetActive(false);


	}
}
