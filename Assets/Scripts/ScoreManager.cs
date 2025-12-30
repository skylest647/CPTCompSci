using UnityEngine;
using System.Collections.Generic;
public class ScoreManager
{
    private JokerManager JokerManager;

    public ScoreManager(JokerManager jokerManager)
    {
        JokerManager = jokerManager;
    }

    public int CalculateFinalScore(HandResult result, List<Card> hand)
    {
        int baseChips = GetBaseChips(result.Type);
        float baseMult = GetBaseMultiplier(result.Type);

        int jokerChips = JokerManager.GetTotalBonusChips(result, hand);
        float jokerMult = JokerManager.GetTotalBonusMultiplier(result, hand);

        int finalScore = (int)((baseChips + jokerChips) * (baseMult + jokerMult));
        return finalScore;
    }
    
//Used AI to find a cleaner way than if, else if 
    private int GetBaseChips(HandType type)
    {
        switch (type)
        {
            case HandType.Pair: return 10;
            case HandType.TwoPair: return 20;
            case HandType.ThreeOfAKind: return 30;
            case HandType.Straight: return 40;
            case HandType.Flush: return 50;
            case HandType.FullHouse: return 60;
            case HandType.FourOfAKind: return 80;
            case HandType.StraightFlush: return 100;
            default: return 5; 
        }
    }

    private float GetBaseMultiplier(HandType type)
    {
        switch (type)
        {
            case HandType.Pair: return 2f;
            case HandType.TwoPair: return 2f;
            case HandType.ThreeOfAKind: return 3f;
            case HandType.Straight: return 4f;
            case HandType.Flush: return 4f;
            case HandType.FullHouse: return 4f;
            case HandType.FourOfAKind: return 7f;
            case HandType.StraightFlush: return 8f;
            default: return 1f;
        }
    }
}
