using System;
using UnityEngine;
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

    private DeckManager DeckManager;
    private HandManager HandManager;
    private JokerManager JokerManager ;
    private ScoreManager ScoreManager;
    private EconomyManager EconomyManager;


    public void begin()
    {
        InitializeManagers();
        StartNewRun();
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
        IsGameOver = false;

        EconomyManager.SetMoney(10);

        DeckManager.BuildStandardDeck();
        DeckManager.Shuffle();
    }

    private void StartPhase(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.SmallBlind:
            case GamePhase.BigBlind:
            case GamePhase.BossBlind:
                PlayBlind(phase);
                break;
                
            case GamePhase.Shop:
                OpenShop();
                break;

            case GamePhase.GameOver:
                GameOver();
                break;
        }
    }
}