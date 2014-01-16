using UnityEngine;
using System.Collections;

public class RollButton : MonoBehaviour {

	DiceDisplayer displayer;
    private bool firstRoll = true;
    public GameObject[] dice;

    public Die die;

	// Use this for initialization
	void Start () {
		displayer = transform.parent.gameObject.GetComponent<DiceDisplayer>();
        dice = GameObject.FindGameObjectsWithTag("Die");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseUpAsButton() {

        if (firstRoll)
        {
            displayer.RollDice();     
            firstRoll = false;
        }
        else
        {
            if (Die.selectedCounter != 0)
            {
                Debug.Log(Die.selectedCounter);
                GameManager.selectedDieValues = new RollValues(6);

                for (int i = 0; i < dice.Length; i++)
                {
                    die = dice[i].GetComponent<Die>();
                    if (die.selected)
                    {
                        GameManager.selectedDieValues[die.value - 1]++;
                    }
                }

                    GameManager.dieValues.Clear();
                //move selected die
                displayer.RollDice();
                ScoreChecker.SelectedDiceChecker(GameManager.selectedDieValues);
            }
            else
            {
                Debug.Log("Please select dice");
            }
        }
     
	}
}
