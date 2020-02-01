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
        bodyPartCount = System.Enum.GetValues(typeof(BodyType)).Length;
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

    public ItemType[] GetMissingItemTypes()
    {
        List<ItemType> missing = new List<ItemType>();
        for (int i = 0; i < bodyParts.Length; i++)
        {
            if (!bodyParts[i])
            {
                switch (i)
                {
                    case 0: missing.Add(ItemType.Head); break;
                    case 1: missing.Add(ItemType.Torso); break;
                    case 2:
                    case 3: missing.Add(ItemType.Arm); break;
                    case 4:
                    case 5: missing.Add(ItemType.Hand); break;
                    case 6:
                    case 7: missing.Add(ItemType.Leg); break;
                    case 8:
                    case 9: missing.Add(ItemType.Foot); break;
                }
            }
        }
        return missing.ToArray();
    }

    public BodyTpe[] GetMissingBodyTypes()
    {
        List<BodyType> missing = new List<BodyType>();
        for (int i = 0; i < bodyParts.Length; i++)
        {
            if (!bodyParts[i])
                missing.Add((BodyType)i);
        }
        return missing.ToArray();
    }

    public bool AddBodypart(ItemType type)
    {
        switch (type)
        {
            case ItemType.Head: return AddSpecificPart(BodyType.Head);
            case ItemType.Torso: return AddSpecificPart(BodyType.Torso);
            case ItemType.Arm: return CheckBothPositions(BodyType.LeftArm, BodyType.RightArm);
            case ItemType.Hand: return CheckBothPositions(BodyType.LeftHand, BodyType.RightHand);
            case ItemType.Leg: return CheckBothPositions(BodyType.LeftLeg, BodyType.RightLeg);
            case ItemType.Foot: return CheckBothPositions(BodyType.LeftFoot, BodyType.RightFoot);
            default: return false;
        }
    }

    private bool CheckBothPositions(BodyType type1, BodyType type2)
    {
        if (!AddSpecificPart(type1)) return AddSpecificPart(type2);
        return true;
    }

    private bool AddSpecificPart(BodyType type)
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
