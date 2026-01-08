using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public int ante;
    public int phase;
    public int money;
    public int handSize;
    public int handsRemaining;
    public int finalScore;
    public int blindGoal;
    
    public List<CardData> fullDeck;
    public List<CardData> currentDeck;
    
    public List<string> jokerNames;

    public float volume;
}

[System.Serializable]
public class CardData
{
    public string value;
    public string suit;
    
    public CardData(string v, string s)
    {
        value = v;
        suit = s;
    }
    
    public Card ToCard()
    {
        return new Card(value, suit);
    }
    
    public static CardData FromCard(Card card)
    {
        return new CardData(card.GetValue(), card.GetSuit());
    }
}

public static class SaveSystem
{
    private const string SAVE_KEY = "BalatroSave";
    
    public static void SaveGame(GameManager gameManager)
    {
        GameSaveData saveData = new GameSaveData();
        
        saveData.ante = gameManager.GetAnte();
        saveData.phase = (int)gameManager.GetPhase();
        saveData.money = gameManager.GetMoney();
        saveData.handSize = gameManager.GetHandSize();
        saveData.handsRemaining = gameManager.GetHandsRemaining();
        saveData.finalScore = gameManager.GetFinalScore();
        saveData.blindGoal = gameManager.GetBlindGoal();
        
        saveData.fullDeck = SaveDeck(gameManager.GetFullDeck());
        saveData.currentDeck = SaveDeck(gameManager.GetCurrentDeck());
        
        saveData.jokerNames = SaveJokers(gameManager.GetActiveJokers());
        
        saveData.volume = PlayerPrefs.GetFloat("Volume", 0.75f);
        
        string json = JsonUtility.ToJson(saveData, true);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
        
        Debug.Log("Game Saved!");
    }
    
    public static bool LoadGame(GameManager gameManager)
    {
        if (!HasSaveData())
        {
            Debug.Log("No save data found");
            return false;
        }
        
        string json = PlayerPrefs.GetString(SAVE_KEY);
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);
        
        gameManager.LoadGameState(
            saveData.ante,
            (GamePhase)saveData.phase,
            saveData.money,
            saveData.handSize,
            saveData.handsRemaining,
            saveData.finalScore,
            saveData.blindGoal
        );
        
        gameManager.LoadDeck(
            LoadDeck(saveData.fullDeck),
            LoadDeck(saveData.currentDeck)
        );
        
        gameManager.LoadJokers(LoadJokers(saveData.jokerNames));
        
        PlayerPrefs.SetFloat("Volume", saveData.volume);
        AudioListener.volume = saveData.volume;
        
        Debug.Log("Game Loaded!");
        return true;
    }
    
    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.Save();
        Debug.Log("Save deleted!");
    }
    
    public static bool HasSaveData()
    {
        return PlayerPrefs.HasKey(SAVE_KEY);
    }
    
    private static List<CardData> SaveDeck(List<Card> deck)
    {
        List<CardData> cardDataList = new List<CardData>();
        foreach (Card card in deck)
        {
            cardDataList.Add(CardData.FromCard(card));
        }
        return cardDataList;
    }
    
    private static List<Card> LoadDeck(List<CardData> cardDataList)
    {
        List<Card> deck = new List<Card>();
        foreach (CardData cardData in cardDataList)
        {
            deck.Add(cardData.ToCard());
        }
        return deck;
    }
    
    private static List<string> SaveJokers(List<Joker> jokers)
    {
        List<string> jokerNames = new List<string>();
        foreach (Joker joker in jokers)
        {
            jokerNames.Add(joker.GetName());
        }
        return jokerNames;
    }
    
    private static List<Joker> LoadJokers(List<string> jokerNames)
    {
        List<Joker> jokers = new List<Joker>();
        
        foreach (string name in jokerNames)
        {
            Joker joker = CreateJokerByName(name);
            if (joker != null)
                jokers.Add(joker);
        }
        
        return jokers;
    }
    
    private static Joker CreateJokerByName(string name)
    {
        switch (name)
        {
            case "Joker": return new JokerJoker();
            case "Greedy Joker": return new GreedyJoker();
            case "Lusty Joker": return new LustyJoker();
            case "Wrathful Joker": return new WrathfulJoker();
            case "Gluttonous Joker": return new GluttonousJoker();
            case "Jolly Joker": return new JollyJoker();
            case "Zany Joker": return new ZanyJoker();
            case "Mad Joker": return new MadJoker();
            case "Crazy Joker": return new CrazyJoker();
            case "Devoted Joker": return new DevotedJoker();
            case "Sly Joker": return new SlyJoker();
            case "Wily Joker": return new WilyJoker();
            case "Clever Joker": return new CleverJoker();
            case "Devious Joker": return new DeviousJoker();
            case "Crafty Joker": return new CraftyJoker();
            case "Half Joker": return new HalfJoker();
            case "Stuntman": return new StuntmanJoker();
            case "Raised Fist": return new RaisedFist();
            case "Scared Face": return new ScaredFace();
            case "Abstract Joker": return new AbstractJoker();
            default:
                Debug.LogWarning($"Unknown joker: {name}");
                return null;
        }
    }
}