using UnityEngine;
using System.Collections.Generic;

public class HandManager
{
    private List<Card> Hand; 

    public HandManager() {
        Hand = new List<Card>();
    }

    public void AddCard(Card card){
        Hand.Add(card);
    }

    public void ClearHand(){
        Hand.Clear();
    }

    public List<Card> GetHand(){
        return Hand;
    }

    public int CardsInHand(){
        return Hand.Count;
    }

    public void SortHand(){
        for (int i = 0; i < Hand.Count - 1; i++)
        {
            for (int j = 0; j < Hand.Count - i - 1; j++)
            {
                if (Hand[j].GetNumericValue() > Hand[j + 1].GetNumericValue())
                {
                    Card temp = Hand[j];
                    Hand[j] = Hand[j + 1];
                    Hand[j + 1] = temp;
                }
            }
        }
    }
    public int FindCardIndex(int value, CardSuit suit){
        for (int i = 0; i < Hand.Count; i++)
        {
            Card card = Hand[i];

            if (card.GetNumericValue() == value && card.GetSuit() == suit)
            {
                return i; 
            }
        }
        return -1; 
    }
}