using System;
using UnityEngine;
//Used Chat Gpt to learn enum
public enum GamePhase
{
    SmallBlind,
    Shop,
    BigBlind,
    BossBlind,
    GameOver
}
public class GameManager : MonoBehaviour
{
    private GamePhase Phase;
    private int Ante;
    private bool IsGameOver;

    // private DeckManager DeckManager;
    // private HandManager HandManager;
    // private JokerManager JokerManager ;
    // private ScoreManager ScoreManager;
    // private BlindManager BlindManager;
    // private EconomyManager EconomyManager;
    // private EventManager EventManager;


    public void begin()
    {
        InitializeManagers();
        StartNewRun();
    }


    private void InitializeManagers()
    {
        // EventManager = new EventManager();

        // DeckManager = new DeckManager();
        // HandManager = new HandManager();
        // ScoreManager = new ScoreManager();
        // BlindManager = new BlindManager();
        // EconomyManager = new EconomyManager();
        // JokerManager = new JokerManager(EventManager);


        // JokerManager.SetGame(this);
        // BlindManager.SetGame(this);
    }

    public void StartNewRun()
    {
        // Ante = 1;
        // Phase = GamePhase.SmallBlind;
        // IsGameOver = false;

        // EconomyManager.SetMoney(10);

        // DeckManager.BuildStandardDeck();
        // DeckManager.Shuffle();

        // BlindManager.StartBlind(Phase, Ante);
        // EventManager.RaiseRunStarted();
    }
}