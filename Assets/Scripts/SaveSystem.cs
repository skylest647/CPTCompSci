using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameSaveData { public int ante, phase, money, handSize, handsRemaining, finalScore, blindGoal; public List<CardData> fullDeck, currentDeck; public List<string> jokerNames; }

[System.Serializable]
public class CardData { public string value, suit; public CardData(string v, string s) { value = v; suit = s; } public Card ToCard() => new Card(suit, value); public static CardData FromCard(Card c) => new CardData(c.GetValue(), c.GetSuit()); }

public static class SaveSystem
{
    public static void SaveGame(GameManager gm)
    {
        GameSaveData data = new GameSaveData { ante = gm.GetAnte(), phase = (int)gm.GetPhase(), money = gm.GetMoney(), handSize = gm.GetHandSize(), handsRemaining = gm.GetHandsRemaining(), finalScore = gm.GetFinalScore(), blindGoal = gm.GetBlindGoal(), fullDeck = new List<CardData>(), currentDeck = new List<CardData>(), jokerNames = new List<string>() };
        foreach (Card c in gm.GetFullDeck()) data.fullDeck.Add(CardData.FromCard(c));
        foreach (Card c in gm.GetCurrentDeck()) data.currentDeck.Add(CardData.FromCard(c));
        foreach (Joker j in gm.GetActiveJokers()) data.jokerNames.Add(j.GetName());
        PlayerPrefs.SetString("BalatroSave", JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    public static bool LoadGame(GameManager gm)
    {
        if (!PlayerPrefs.HasKey("BalatroSave")) return false;
        GameSaveData data = JsonUtility.FromJson<GameSaveData>(PlayerPrefs.GetString("BalatroSave"));
        gm.LoadGameState(data.ante, (GamePhase)data.phase, data.money, data.handSize, data.handsRemaining, data.finalScore, data.blindGoal);
        List<Card> full = new List<Card>(), curr = new List<Card>();
        foreach (CardData d in data.fullDeck) full.Add(d.ToCard());
        foreach (CardData d in data.currentDeck) curr.Add(d.ToCard());
        gm.LoadDeck(full, curr);
        List<Joker> jokers = new List<Joker>();
        foreach (string n in data.jokerNames) { Joker j = CreateJokerByName(n); if (j != null) jokers.Add(j); }
        gm.LoadJokers(jokers);
        return true;
    }

    public static bool HasSaveData() => PlayerPrefs.HasKey("BalatroSave");
    public static void DeleteSave() => PlayerPrefs.DeleteKey("BalatroSave");

    private static Joker CreateJokerByName(string n)
    {
        switch (n) {
            case "Joker": return new JokerJoker(); case "Greedy Joker": return new GreedyJoker(); case "Lusty Joker": return new LustyJoker(); case "Wrathful Joker": return new WrathfulJoker();
            case "Gluttonous Joker": return new GluttonousJoker(); case "Jolly Joker": return new JollyJoker(); case "Zany Joker": return new ZanyJoker(); case "Mad Joker": return new MadJoker();
            case "Crazy Joker": return new CrazyJoker(); case "Devoted Joker": return new DevotedJoker(); case "Sly Joker": return new SlyJoker(); case "Wily Joker": return new WilyJoker();
            case "Clever Joker": return new CleverJoker(); case "Devious Joker": return new DeviousJoker(); case "Crafty Joker": return new CraftyJoker(); case "Half Joker": return new HalfJoker();
            case "Stuntman": return new StuntmanJoker(); case "Raised Fist": return new RaisedFist(); case "Scared Face": return new ScaredFace(); case "Abstract Joker": return new AbstractJoker();
            default: return null;
        }
    }
}