using System;
using System.Collections.Generic;

public class DeckManager
{
    private List<Card> FullDeck;      
    private List<Card> CurrentDeck;   
    private System.Random rng = new System.Random(); 

    public DeckManager()
    {
        FullDeck = new List<Card>();
        CurrentDeck = new List<Card>();
    }

    public void BuildStandardDeck()
    {
        FullDeck.Clear();
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        foreach (string suit in suits)
        {
            foreach (string value in values)
            {
                FullDeck.Add(new Card(suit, value));
            }
        }
    }

    public void RefillDeck()
    {
        CurrentDeck = new List<Card>(FullDeck);
        Shuffle();
    }

    public void Shuffle()
    {
        int n = CurrentDeck.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            Card temp = CurrentDeck[i];
            CurrentDeck[i] = CurrentDeck[j];
            CurrentDeck[j] = temp;
        }
    }

    public Card Draw()
    {
        if (CurrentDeck.Count == 0) return null;
        Card top = CurrentDeck[0];
        CurrentDeck.RemoveAt(0);
        return top;
    }

    public List<Card> GetFullDeck() => FullDeck;
    public List<Card> GetCurrentDeck() => CurrentDeck;
    public void LoadDecks(List<Card> full, List<Card> current)
    {
        FullDeck = full;
        CurrentDeck = current;
    }
}