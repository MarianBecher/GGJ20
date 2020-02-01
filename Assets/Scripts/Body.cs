using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body
{
    private const int defaultParts = 7;
    private int bodyPartCount;
    private bool[] bodyParts;

    public Body() : this(defaultParts) { }

    public Body(int amount)
    {
        bodyPartCount = System.Enum.GetValues(typeof(ItemType)).Length;
        bodyParts = new bool[bodyPartCount];
        GenerateBody(amount);
    }

    private void GenerateBody(int amountOfParts)
    {
        if (amountOfParts > bodyPartCount) amountOfParts = bodyPartCount;

        HashSet<(int, bool)> generatedParts = new HashSet<(int, bool)>();
        while (amountOfParts > 0)
        {
            (int, bool) temp = (Random.Range(0, bodyPartCount), true);
            if (!generatedParts.Contains(temp))
            {
                generatedParts.Add(temp);
                amountOfParts--;
            }
        }
        foreach ((int, bool) generatedPart in generatedParts)
        {
            bodyParts[generatedPart.Item1] = generatedPart.Item2;
        }
    }

    public bool AddBodypart(ItemType type)
    {
        if (!bodyParts[(int)type])
        {
            bodyParts[(int)type] = true;
            return true;
        }
        return false;
    }

    public bool BodyIsComplete()
    {
        foreach (bool hasPart in bodyParts)
        {
            if (!hasPart) return false;
        }
        return true;
    }
}
