using UnityEngine;
using System.Collections;

public class ScoreChecker : MonoBehaviour
{
    public RollValues dieValues;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void InitialChecker()
    {
        if (!CheckForOnesAndFives(GameManager.dieValues) && !CheckForMultiples(GameManager.dieValues) && !CheckForThreePairs(GameManager.dieValues) && !CheckForStraight(GameManager.dieValues))
        {
            Debug.Log("You busted!");
            GameManager.score = 0;
            //end turn
        }
    }

    public static void SelectedDiceChecker(RollValues dieValues)
    {
        bool passed = false;
        if (Die.selectedCounter == 6)
        {
            if (CheckForStraight(dieValues))
            {
                passed = true;
                Debug.Log("You got a straight!");
            }
            else if (CheckForThreePairs(dieValues))
            {
                passed = true;
                Debug.Log("You got three pairs!");
            }

        }

        if (Die.selectedCounter >= 3)
            if (CheckForMultiples(dieValues))
            {
                passed = true;
                Debug.Log("You got multiples!");
            }
        if (Die.selectedCounter >= 1)
        {
            if (CheckForOnesAndFives(dieValues))
            {
                passed = true;
                Debug.Log("You got ones/fives!");
            }
        }

        if (!passed)
        {
            Debug.Log("The Dice you have selected are invalid");
        }

    }





    /*********************************************************************/

    private static bool CheckForStraight(RollValues dieValues)
    {
        for (int i = 0; i < dieValues.Length; i++)
        {
            if (dieValues[i] != 1)
                return false;

        }
        //GameManager.score += 1500;
        GameManager.dieValues.Clear();
        if (GameManager.selectedDieValues != null)
            GameManager.selectedDieValues.Clear();

        return true;
    }

    private static bool CheckForThreePairs(RollValues dieValues)
    {
        for (int i = 0; i < dieValues.Length; i++)
        {
            if (dieValues[i] == 2)
            {
                for (int j = 0; j < dieValues.Length; j++)
                {
                    if (j == i) continue;
                    if (dieValues[j] == 2)
                    {
                        for (int k = 0; k < dieValues.Length; k++)
                        {
                            if (k == i || k == j) continue;
                            if (dieValues[k] == 2)
                            {
                                //GameManager.score += 750;
                            }
                            GameManager.dieValues.Clear();
                            if (GameManager.selectedDieValues != null)
                                GameManager.selectedDieValues.Clear();

                        }
                        return false;
                    }

                }
                return false;
            }
        }
        return false;
    }

    private static bool CheckForMultiples(RollValues dieValues)
    {
        for (int i = 0; i < dieValues.Length; i++)
        {
            if (dieValues[i] >= 3)
            {
                if (i == 0)
                {
                    //GameManager.score += (1000) + (GameManager.dieValues[0] - 3 * (1000) )
                    GameManager.dieValues[i] = 0;
                    if (GameManager.selectedDieValues != null)
                        GameManager.selectedDieValues[i] = 0;

                    return true;

                }
                else
                {
                    //GameManager.score += ( (i + 1) * 100) + (GameManager.dieValues[i] - 3 * (i + 1) * 100)
                    GameManager.dieValues[i] = 0;
                    if (GameManager.selectedDieValues != null)
                        GameManager.selectedDieValues[i] = 0;

                    return true;
                }
            }


        }
        return false;
    }

    private static bool CheckForOnesAndFives(RollValues dieValues)
    {
        bool found = false;
        if (dieValues[0] != 0)
        {
            //numberOfOnes = GameManager.dieValues[0];
            //GameManager.score += numberOfOnes * 100;
            
            GameManager.dieValues[0] = 0;
            if (GameManager.selectedDieValues != null)
                GameManager.selectedDieValues[0] = 0;

            found = true;
        }

        if (dieValues[4] != 0)
        {
            //numberOfFives = GameManager.dieValues[4];
            //GameManager.score += numberOfFives * 50;
            GameManager.dieValues[4] = 0;
            if (GameManager.selectedDieValues != null)
                GameManager.selectedDieValues[4] = 0;

            found = true;
        }
        return found;
    }
}
