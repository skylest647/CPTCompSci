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

    public void RemoveCard(Card card){
        Hand.Remove(card); 
    }

    public void ClearHand(){
        Hand.Clear();
    }

    public int CardsInHand(){
        return Hand.Count;
    }

    public void PrintHand(){
        foreach (var card in Hand)
        {
            Debug.Log(card.ToString());
        }
    }

    public bool ContainsCard(Card card){
        return Hand.Contains(card);
    }
    private int GetValueRank(string value){
        string[] order = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        for (int i = 0; i < order.Length; i++)
        {
            if (order[i] == value)
                return i;
        }
        return -1;
    }

    private int GetSuitRank(string suit){
        string[] order = { "Clubs", "Diamonds", "Hearts", "Spades" };
        for (int i = 0; i < order.Length; i++)
        {
            if (order[i] == suit)
                return i;
        }
        return -1;
    }
    //Bubble sort to sort hand 
    public void SortHand(){
        for (int i = 0; i < Hand.Count - 1; i++)
        {
            for (int j = 0; j < Hand.Count - i - 1; j++)
            {
                if (ShouldSwap(Hand[j], Hand[j + 1]))
                {
                    Card temp = Hand[j];
                    Hand[j] = Hand[j + 1];
                    Hand[j + 1] = temp;
                }
            }
        }
    }
    private bool ShouldSwap(Card a, Card b){
        int valueA = GetValueRank(GetCardValue(a));
        int valueB = GetValueRank(GetCardValue(b));

        if (valueA > valueB)
            return true;

        if (valueA < valueB)
            return false;

        int suitA = GetSuitRank(GetCardSuit(a));
        int suitB = GetSuitRank(GetCardSuit(b));

        return suitA > suitB;
    }
    private string GetCardValue(Card card)
    {
        return card.GetValue();
    }

    private string GetCardSuit(Card card)
    {
        return card.GetSuit();
    }
    public List<Card> GetHand()
    {
        return this.Hand;
    } 
}
