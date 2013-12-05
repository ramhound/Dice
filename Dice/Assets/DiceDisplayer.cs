using UnityEngine;
using System.Collections;

public class DiceDisplayer : MonoBehaviour {
    private Die[] dice { get; set; }
    public static bool rolling;

    // Use this for initialization
    void Start() {
        dice = transform.Find("Dice").FindChildrenByBeginningOfName<Die>("Die");
    }

    public void RollDice() {
        foreach(Die die in dice)
            die.ShowDie(true);

        if(!rolling) {
            rolling = true;

            foreach(Die die in dice)
                die.StartRolling();
        }
    }
}
