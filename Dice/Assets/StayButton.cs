using UnityEngine;
using System.Collections;

public class StayButton : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseUpAsButton()
    {
        GameManager.dieValues.Clear();
        //GameManager.totalScore += GameManager.score;
        //GameManager.score = 0;
        //check if removed dice are legal
        //remove selected dice

    }
}
