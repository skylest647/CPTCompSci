using UnityEngine;
using System.Collections.Generic;
public enum GamePhase
{
    SmallBlind,
    BigBlind,
    BossBlind
}
public class GameManager : MonoBehaviour
{
    private GamePhase Phase;
    private int Ante;
    private bool IsGameOver;
    private bool InShop;
    private int HandSize;
    private int BlindGoal;
    private int HandsRemaining;
    private int FinalScore;
    private int hands = 4;
    private List<Card> PlayedHand;

    private DeckManager DeckManager;
    private HandManager HandManager;
    private JokerManager JokerManager ;
    private ScoreManager ScoreManager;
    private EconomyManager EconomyManager;


    public void Start()
    {
        InitializeManagers();
    }


    private void InitializeManagers()
    {
        DeckManager = new DeckManager();
        HandManager = new HandManager();
        EconomyManager = new EconomyManager();
        JokerManager = new JokerManager();
        ScoreManager = new ScoreManager(JokerManager);
    }

    public void StartNewRun()
    {
        Ante = 1;
        Phase = GamePhase.SmallBlind;
        SetBlindTarget();
        IsGameOver = false;
        InShop = false;
        HandSize = 7;
        HandsRemaining = hands;
        PlayedHand = new List<Card>();

        EconomyManager.SetMoney(10);

        DeckManager.BuildStandardDeck();
        DeckManager.Shuffle();
        
        JokerManager = new JokerManager();
        ScoreManager = new ScoreManager(JokerManager);
        
        DrawHand();
        SaveGame();
    }
    public void DrawHand()
    {
        HandManager.ClearHand();
        for (int i = 0; i < HandSize; i++)
        {
            Card card = DeckManager.Draw();

            if (card == null)
            {
                IsGameOver = true;
                break;
            }

            HandManager.AddCard(card);
        }
        if (HandManager.CardsInHand() > 0)
        {
            HandManager.SortHand();
        }
    }
    //selectedCardIndicies will be from a user input I will somehow figure out
    
    public void PlayHand(List<int> selectedCardIndices)
    {
        
        if (HandManager.CardsInHand() == 0){
            IsGameOver = true;
            return;
        }
        var hand = HandManager.GetHand();

        foreach (int index in selectedCardIndices){
            if (index >= 0 && index < hand.Count)
            {
                PlayedHand.Add(hand[index]);
            }
        }
        
        
        HandEvaluator evaluator = new HandEvaluator();
        HandResult result = evaluator.Evaluate(PlayedHand);

        int Score = ScoreManager.CalculateFinalScore(result, PlayedHand);

        FinalScore += Score;
        PlayedHand.Clear();
        HandsRemaining--;

        if (HandsRemaining < 1)
            {
                EvaluateBlind();
            }
        if (!IsGameOver)
            {
                DrawHand();
            }
        
        SaveGame();
    }
    public void SetBlindTarget()
    {
        switch (Phase)
        {
            case GamePhase.SmallBlind: BlindGoal = 20 * Ante; break;
            case GamePhase.BigBlind: BlindGoal = 30 * Ante; break;
            case GamePhase.BossBlind: BlindGoal = 50 * Ante; break;
        }
    }
    private void EvaluateBlind()
    {
        if (FinalScore >= BlindGoal)
        {
            FinalScore = 0;
            HandsRemaining = hands;
            DeckManager.RefillDeck();
            AdvanceBlind();
        }
        else
        {
            IsGameOver = true;
        }
    }
    private void AdvanceBlind(){
        switch (Phase)
            {
                case GamePhase.SmallBlind: Phase = GamePhase.BigBlind; break;
                case GamePhase.BigBlind: Phase = GamePhase.BossBlind; break;
                case GamePhase.BossBlind:
                    Phase = GamePhase.SmallBlind; 
                    Ante++;
                    break;
            }
        SetBlindTarget();

    }
    public void Update()
    {
        if (IsGameOver)
        {
            return;
        }
    }
    public void PauseGame()
    {
        return;
        //Pause menu functionality here
    }
    
    public int GetAnte() { return Ante; }
    public GamePhase GetPhase() { return Phase; }
    public int GetMoney() { return EconomyManager.GetMoney(); }
    public int GetHandSize() { return HandSize; }
    public int GetHandsRemaining() { return HandsRemaining; }
    public int GetFinalScore() { return FinalScore; }
    public int GetBlindGoal() { return BlindGoal; }

    public List<Card> GetFullDeck()
    {
        return DeckManager.GetFullDeck();
    }

    public List<Card> GetCurrentDeck()
    {
        return DeckManager.GetCurrentDeck();
    }

    public List<Joker> GetActiveJokers()
    {
        return JokerManager.GetActiveJokers();
    }

    public void LoadGameState(int ante, GamePhase phase, int money, int handSize, int handsRemaining, int finalScore, int blindGoal)
    {
        Ante = ante;
        Phase = phase;
        HandSize = handSize;
        HandsRemaining = handsRemaining;
        FinalScore = finalScore;
        BlindGoal = blindGoal;
        IsGameOver = false;
        InShop = false;

        EconomyManager.SetMoney(money);
    }

    public void LoadDeck(List<Card> fullDeck, List<Card> currentDeck)
    {
        DeckManager.LoadDecks(fullDeck, currentDeck);
    }

    public void LoadJokers(List<Joker> jokers)
    {
        JokerManager.LoadJokers(jokers);
        ScoreManager = new ScoreManager(JokerManager);
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(this);
    }

    public void LoadGame()
    {
        bool success = SaveSystem.LoadGame(this);
        if (success)
        {
            DrawHand();
        }
    }
}