using UnityEngine;
using System.Collections;

public class RollValues
{

    int[] dieValues;

    public RollValues(int numberOfDice)
    {
        dieValues = new int[numberOfDice];
    }

    public int Length { get { return dieValues.Length; } }

    public int this[int index]
    {
        get { return dieValues[index]; }
        set { dieValues[index] = value; }
    }

    public void Clear()
    {
        for (int i = 0; i < dieValues.Length; i++)
        {
            dieValues[i] = 0;
        }

    }

}
