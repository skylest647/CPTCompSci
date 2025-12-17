using System;
using System.Collections.Generic;
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
}

public class DeckManager : MonoBehaviour
{
    private List<Card> FullDeck;      // Permanent deck with all cards
    private List<Card> CurrentDeck;   // Deck for the current hand/blind
    private System.Random rng = new System.Random(); // For shuffling

    void Start()
    {
        BuildStandardDeck();
        RefillDeck();
    }

    public void BuildStandardDeck()
    {
        FullDeck = new List<Card>();

        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        foreach (var suit in suits)
        {
            foreach (var value in values)
            {
                FullDeck.Add(new Card(value, suit));
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

    public void AddCard(Card card, bool addToCurrentDeck = true)
    {
        FullDeck.Add(card);
        if (addToCurrentDeck)
            CurrentDeck.Add(card);
    }

    public bool RemoveCard(Card card)
    {
        bool removed = FullDeck.Remove(card);
        CurrentDeck.Remove(card);
        return removed;
    }

    public int CardsRemaining()
    {
        return CurrentDeck.Count;
    }
}
