using UnityEngine;

public class HandResult
{
    public HandType Type;
    public int Multiplier;

    public HandResult(HandType type, int multiplier)
    {
        Type = type;
        Multiplier = multiplier;
    }
}