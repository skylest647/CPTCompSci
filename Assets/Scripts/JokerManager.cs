using UnityEngine;
using System.Collections.Generic;

public class JokerManager
{
    private List<Joker> ActiveJokers;

    public JokerManager()
    {
        ActiveJokers = new List<Joker>();
    }

    public void AddJoker(Joker joker)
    {
        ActiveJokers.Add(joker);
    }

    public int GetTotalBonusChips(HandResult result, List<Card> hand)
    {
        int total = 0;

        foreach (Joker joker in ActiveJokers)
        {
            total += joker.GetBonusChips(result, hand);
        }

        return total;
    }

    public float GetTotalBonusMultiplier(HandResult result, List<Card> hand)
    {
        float total = 0f;

        foreach (Joker joker in ActiveJokers)
        {
            total += joker.GetBonusMultiplier(result, hand);
        }

        return total;
    }
}