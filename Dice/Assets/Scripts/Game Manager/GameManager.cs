using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance { get; private set; }

    public static RollValues dieValues;
    public static int score;
    public static int totalScore;

    void Awake() 
    {
        instance = this;
        dieValues = new RollValues(6);
        totalScore = 0;
    }

	// Use this for initialization
	void Start () {
        //GameObject[] dice = gameObject.FindChildrenByBeginningOfNameRecursively("Die");
        //foreach(GameObject go in dice)
        //    go.SetActive(false);


	}
}
