using System.Collections.Generic;
using UnityEngine;

public enum HandType
{
    HighCard,
    Pair,
    TwoPair,
    ThreeOfAKind,
    Straight,
    Flush,
    FullHouse,
    FourOfAKind,
    StraightFlush
}

public class HandEvaluator
{
    public HandResult Evaluate(List<Card> hand)
    {
        if (hand == null || hand.Count == 0)
            return new HandResult(HandType.HighCard, 1);

        Dictionary<string, int> valueCounts = CountValues(hand);

        // Strongest hands first
        if (IsStraight(hand) && IsFlush(hand))
            return new HandResult(HandType.StraightFlush, 9);

        if (IsFourOfAKind(valueCounts))
            return new HandResult(HandType.FourOfAKind, 7);

        if (IsFullHouse(valueCounts))
            return new HandResult(HandType.FullHouse, 6);

        if (IsFlush(hand))
            return new HandResult(HandType.Flush, 5);

        if (IsStraight(hand))
            return new HandResult(HandType.Straight, 5);

        if (IsThreeOfAKind(valueCounts))
            return new HandResult(HandType.ThreeOfAKind, 4);

        if (IsTwoPair(valueCounts))
            return new HandResult(HandType.TwoPair, 3);

        if (IsPair(valueCounts))
            return new HandResult(HandType.Pair, 2);

        return new HandResult(HandType.HighCard, 1);
    }


    private Dictionary<string, int> CountValues(List<Card> hand)
    {
        Dictionary<string, int> counts = new Dictionary<string, int>();
        foreach (Card card in hand)
        {
            string value = card.GetValue();
            if (!counts.ContainsKey(value))
                counts[value] = 0;
            counts[value]++;
        }
        return counts;
    }

    private bool IsPair(Dictionary<string, int> counts)
    {
        foreach (int count in counts.Values)
            if (count == 2) return true;
        return false;
    }

    private bool IsTwoPair(Dictionary<string, int> counts)
    {
        int pairCount = 0;
        foreach (int count in counts.Values)
            if (count == 2) pairCount++;
        return pairCount == 2;
    }

    private bool IsThreeOfAKind(Dictionary<string, int> counts)
    {
        foreach (int count in counts.Values)
            if (count == 3) return true;
        return false;
    }

    private bool IsFullHouse(Dictionary<string, int> counts)
    {
        bool hasThree = false;
        bool hasPair = false;
        foreach (int count in counts.Values)
        {
            if (count == 3) hasThree = true;
            if (count == 2) hasPair = true;
        }
        return hasThree && hasPair;
    }

    private bool IsFourOfAKind(Dictionary<string, int> counts)
    {
        foreach (int count in counts.Values)
            if (count == 4) return true;
        return false;
    }

    private bool IsFlush(List<Card> hand)
    {
        string suit = hand[0].GetSuit();
        foreach (var card in hand)
            if (card.GetSuit() != suit) return false;
        return true;
    }

    private bool IsStraight(List<Card> hand)
    {
        List<int> values = hand.ConvertAll(card => card.GetNumericValue());
        values.Sort();

        // Check normal straight
        bool normalStraight = true;
        for (int i = 1; i < values.Count; i++)
            if (values[i] != values[i - 1] + 1) normalStraight = false;

        // Check Ace-low straight (A=1)
        bool aceLowStraight = false;
        if (values.Contains(14))
        {
            List<int> lowValues = values.ConvertAll(v => v == 14 ? 1 : v);
            lowValues.Sort();
            aceLowStraight = true;
            for (int i = 1; i < lowValues.Count; i++)
                if (lowValues[i] != lowValues[i - 1] + 1) aceLowStraight = false;
        }

        return normalStraight || aceLowStraight;
    }
}
