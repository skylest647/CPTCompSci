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
        for (int i = 0; i < Hand.Count - 1; i++){
            for (int j = 0; j < Hand.Count - i - 1; j++){
                bool swap = false;
                if (Hand[j].GetNumericValue() > Hand[j + 1].GetNumericValue()){
                    swap = true;
                }
                else if (Hand[j].GetNumericValue() == Hand[j + 1].GetNumericValue())
                {
                    if (SuitOrder(Hand[j].GetSuit()) > SuitOrder(Hand[j + 1].GetSuit())){
                        swap = true;
                    }
                }
                if (swap)
                {
                    Card temp = Hand[j];
                    Hand[j] = Hand[j + 1];
                    Hand[j + 1] = temp;
                }
            }
        }
    }

    public int FindCardIndex(int value, string suit){
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
    private int SuitOrder(string suit){
        switch (suit)
        {
            case "Clubs": return 1;
            case "Diamonds": return 2;
            case "Hearts": return 3;
            case "Spades": return 4;
            default: return 0;
        }
    }
}