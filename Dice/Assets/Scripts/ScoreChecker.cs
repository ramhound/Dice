using UnityEngine;
using System.Collections;

public class ScoreChecker : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void Checker()
    {
        for (int i = 0; i < GameManager.dieValues.Length; i++)
        {
            //Debug.Log(GameManager.dieValues[i]);
        }

        if (CheckForStraight())
            Debug.Log("You got a straight!");
        if (CheckForThreePairs())
            Debug.Log("You got three pairs!");
    }

    /*********************************************************************/

    private static bool CheckForStraight()
    {
        for (int i = 0; i < GameManager.dieValues.Length; i++)
        {
            if (GameManager.dieValues[i] != 1)
                return false;

        }
        //GameManager.score += 1500;
        return true;
    }

    private static bool CheckForThreePairs()
    {
        for (int i = 0; i < GameManager.dieValues.Length; i++)
        {
            if (GameManager.dieValues[i] == 2)
            {
                for (int j = 0; j < GameManager.dieValues.Length; j++)
                {
                    if (j == i) continue;
                    if (GameManager.dieValues[j] == 2)
                    {
                        for (int k = 0; k < GameManager.dieValues.Length; k++)
                        {
                            if (k == i || k == j) continue;
                            if (GameManager.dieValues[k] == 2)
                                //GameManager.score += 750;
                                return true;
                        }
                        return false;
                    }

                }
                return false;
            }
        }
        return false;
    }

    private static bool CheckForMultiples()
    {
        for (int i = 0; i < GameManager.dieValues.Length; i++)
        {
            if (GameManager.dieValues[i] >= 3)
            {
                if (i == 0)
                {
                    //GameManager.score += (1000) + (GameManager.dieValues[0] - 3 * (1000) )
                    return true;

                }
                else
                {  
                    //GameManager.score += ( (i + 1) * 100) + (GameManager.dieValues[i] - 3 * (i + 1) * 100)
                    return true;
                }
            }


        }
        return false;
    }

    private static bool CheckForOnes()
    {
        if (GameManager.dieValues[0] != 0)
        {
            //numberOfOnes = GameManager.dieValues[0];
            //GameManager.score += numberOfOnes * 100;
            return true;
        }
        return false;
    }

    private static bool CheckForFives()
    {
        if (GameManager.dieValues[4] != 0)
        {
            //numberOfFives = GameManager.dieValues[4];
            //GameManager.score += numberOfFives * 50;
            return true;
        }
        return false;
    }
}
