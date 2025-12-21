using UnityEngine;

public class Card
{
    private string Value;
    private string Suit;

    public Card(string value, string suit)
    {
        Value = value;
        Suit = suit;
    }

    public override string ToString()
    {
        return Value + " of " + Suit;
    }
    public string GetValue()
    {
        return Value;
    }

    public string GetSuit()
    {
        return Suit;
    }
    public int GetNumericValue()
    {
        switch (Value)
        {
            case "2": return 2;
            case "3": return 3;
            case "4": return 4;
            case "5": return 5;
            case "6": return 6;
            case "7": return 7;
            case "8": return 8;
            case "9": return 9;
            case "10": return 10;
            case "J": return 11;
            case "Q": return 12;
            case "K": return 13;
            case "A": return 14;
            default: return 0;
        }
    }
}
