using UnityEngine;
using System.Collections.Generic;
using TMPro;

// The enum must be outside the class
public enum GamePhase { SmallBlind, BigBlind, BossBlind }

public class GameManager : MonoBehaviour
{
    private GamePhase Phase;
    private int Ante;
    private int HandSize;
    private int BlindGoal;
    private int HandsRemaining;
    private int FinalScore;
    private int hands = 4;
    
    private bool IsGameOver;
    private bool IsInGame;
    private bool InShop;

    private List<Card> PlayedHand;
    private List<int> selectedIndices = new List<int>();

    // Shop Data
    private Joker shopJoker1;
    private Joker shopJoker2;
    private Card shopCard;
    private int cardCost = 3;
    private bool joker1Bought;
    private bool joker2Bought;
    private bool cardBought;

    // Sub-Managers (The Workers)
    private DeckManager DeckManager;
    private HandManager HandManager;
    private JokerManager JokerManager;
    private ScoreManager ScoreManager;
    private EconomyManager EconomyManager;
    private AllJokers allJokers;

    [Header("Main UI References")]
    public GameObject MainUI;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI anteText;
    public TextMeshProUGUI phaseText;
    public TextMeshProUGUI handSizeText;
    public TextMeshProUGUI handsRemainingText;
    public TextMeshProUGUI blindGoalText;
    public TextMeshProUGUI finalScoreText;
    
    [Header("Shop UI References")]
    public GameObject shopPanel;
    public TextMeshProUGUI joker1Name;
    public TextMeshProUGUI joker1Desc;
    public TextMeshProUGUI joker1Cost;
    public TextMeshProUGUI joker2Name;
    public TextMeshProUGUI joker2Desc;
    public TextMeshProUGUI joker2Cost;
    public TextMeshProUGUI cardCostText;
    public UnityEngine.UI.Image cardImage;

    [Header("Hand Visuals")]
    public Transform handUIParent;
    public GameObject cardUIPrefab;
    public GameObject handUI;
    private List<GameObject> cardUIObjects = new List<GameObject>();
    private float liftAmount = 30f;

    public void Start()
    {
        // Initialize all Worker managers
        DeckManager = new DeckManager();
        HandManager = new HandManager();
        EconomyManager = new EconomyManager();
        JokerManager = new JokerManager();
        ScoreManager = new ScoreManager(JokerManager);
        allJokers = new AllJokers();
    }

    public void Update()
    {
        PauseMenuManager pause = FindFirstObjectByType<PauseMenuManager>();
        if (pause != null)
        {
            if (pause.IsPaused() == true) { return; }
        }

        if (IsGameOver == true)
        {
            IsInGame = false;
            MainUI.SetActive(false);
            return;
        }

        if (IsInGame == true)
        {
            MainUI.SetActive(true);
            UpdateMainUI();
        }
        else
        {
            MainUI.SetActive(false);
        }

        if (InShop == true)
        {
            handUI.SetActive(false);
            shopPanel.SetActive(true);
            HandleShopInput(); 
            return;
        }
        else
        {
            if (IsInGame == true) { handUI.SetActive(true); }
            shopPanel.SetActive(false);
        }

        // Gameplay selection
        if (Input.GetKeyDown(KeyCode.Alpha1)) { ToggleSelection(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { ToggleSelection(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { ToggleSelection(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { ToggleSelection(3); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { ToggleSelection(4); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { ToggleSelection(5); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { ToggleSelection(6); }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedIndices.Count > 0)
            {
                PlayHand(selectedIndices);
            }
        }
    }

    private void HandleShopInput()
    {
        if (Input.GetKeyDown(KeyCode.Q)) { ExitShop(); }
        if (Input.GetKeyDown(KeyCode.Z)) { BuyJoker1(); }
        if (Input.GetKeyDown(KeyCode.X)) { BuyJoker2(); }
        if (Input.GetKeyDown(KeyCode.C)) { BuyCard(); }
    }

    public void StartNewRun()
    {
        // Ensure managers are ready
        if (EconomyManager == null) { Start(); }

        Ante = 1;
        Phase = GamePhase.SmallBlind;
        HandSize = 7;
        BlindGoal = 20;
        HandsRemaining = hands;
        FinalScore = 0;
        EconomyManager.SetMoney(10);
        
        DeckManager.BuildStandardDeck();
        DeckManager.RefillDeck();
        
        IsInGame = true;
        IsGameOver = false;
        InShop = false;
        
        DrawHand();
        RefreshHandUI();
    }

    public void UpdateMainUI()
    {
        if (moneyText != null) { moneyText.text = "Money: $" + EconomyManager.GetMoney(); }
        if (anteText != null) { anteText.text = "Ante: " + Ante; }
        if (handsRemainingText != null) { handsRemainingText.text = "Hands: " + HandsRemaining; }
        if (blindGoalText != null) { blindGoalText.text = "Goal: " + BlindGoal; }
        if (finalScoreText != null) { finalScoreText.text = "Score: " + FinalScore; }
        if (phaseText != null) { phaseText.text = "Phase: " + Phase.ToString(); }
        if (handSizeText != null) { handSizeText.text = "Hand Size: " + HandSize; }
    }

    // --- Loading Logic ---
    public void LoadGame() 
    {
        // 1. Force managers to initialize if they haven't
        if (EconomyManager == null) { Start(); }
        
        // 2. Run the SaveSystem load
        SaveSystem.LoadGame(this);
        
        // 3. Force Game State to be active
        IsInGame = true; 
        IsGameOver = false;
        InShop = false;
        
        // 4. Update Visuals
        UpdateMainUI();
        RefreshHandUI();
    }

    public void LoadGameState(int a, GamePhase p, int m, int hs, int hr, int fs, int bg)
    {
        Ante = a; 
        Phase = p; 
        if (EconomyManager != null) { EconomyManager.SetMoney(m); }
        HandSize = hs; 
        HandsRemaining = hr; 
        FinalScore = fs; 
        BlindGoal = bg;
        
        IsInGame = true; // This makes sure the UI turns on
    }

    public void EnterShop()
    {
        InShop = true;
        joker1Bought = false;
        joker2Bought = false;
        cardBought = false;

        shopJoker1 = allJokers.GetRandomJoker();
        shopJoker2 = allJokers.GetRandomJoker();
        shopCard = GenerateRandomCard();

        if (joker1Name != null) { joker1Name.text = shopJoker1.GetName(); }
        if (joker1Desc != null) { joker1Desc.text = shopJoker1.GetDescription(); }
        if (joker1Cost != null) { joker1Cost.text = "$" + shopJoker1.GetCost(); }

        if (joker2Name != null) { joker2Name.text = shopJoker2.GetName(); }
        if (joker2Desc != null) { joker2Desc.text = shopJoker2.GetDescription(); }
        if (joker2Cost != null) { joker2Cost.text = "$" + shopJoker2.GetCost(); }

        if (cardCostText != null) { cardCostText.text = "$" + cardCost; }
        if (cardImage != null)
        {
            cardImage.sprite = Resources.Load<Sprite>("Cards/" + shopCard.GetSuit() + "_" + shopCard.GetValue());
        }
    }

    public void BuyJoker1()
    {
        if (joker1Bought == false && shopJoker1 != null)
        {
            int cost = shopJoker1.GetCost();
            if (EconomyManager.SpendMoney(cost) == true)
            {
                JokerManager.AddJoker(shopJoker1);
                joker1Bought = true;
                if (joker1Name != null) { joker1Name.text = "BOUGHT"; }
                if (joker1Desc != null) { joker1Desc.text = ""; }
                if (joker1Cost != null) { joker1Cost.text = ""; }
            }
        }
    }

    public void BuyJoker2()
    {
        if (joker2Bought == false && shopJoker2 != null)
        {
            int cost = shopJoker2.GetCost();
            if (EconomyManager.SpendMoney(cost) == true)
            {
                JokerManager.AddJoker(shopJoker2);
                joker2Bought = true;
                if (joker2Name != null) { joker2Name.text = "BOUGHT"; }
                if (joker2Desc != null) { joker2Desc.text = ""; }
                if (joker2Cost != null) { joker2Cost.text = ""; }
            }
        }
    }

    public void BuyCard()
    {
        if (cardBought == false && shopCard != null)
        {
            if (EconomyManager.SpendMoney(cardCost) == true)
            {
                DeckManager.GetFullDeck().Add(shopCard);
                cardBought = true;
                if (cardCostText != null) { cardCostText.text = "SOLD"; }
            }
        }
    }

    public void ExitShop()
    {
        InShop = false;
        DrawHand();
        RefreshHandUI();
    }

    public void PlayHand(List<int> indices)
    {
        PlayedHand = new List<Card>();
        List<Card> currentHand = HandManager.GetHand();
        for (int i = 0; i < indices.Count; i++)
        {
            if (indices[i] < currentHand.Count) { PlayedHand.Add(currentHand[indices[i]]); }
        }

        HandEvaluator eval = new HandEvaluator();
        FinalScore = FinalScore + ScoreManager.CalculateFinalScore(eval.Evaluate(PlayedHand), PlayedHand);
        HandsRemaining = HandsRemaining - 1;
        selectedIndices.Clear();

        EvaluateBlind();
        
        SaveGame();
    }

    private void EvaluateBlind()
    {
        if (FinalScore >= BlindGoal){
                FinalScore = 0;
                HandsRemaining = hands;
                DeckManager.RefillDeck();
                AdvanceBlind();
                EconomyManager.AddMoney(10);
                EnterShop();
            }
            else if (HandsRemaining < 1){
                IsGameOver = true;
            }

        
    }

    private void AdvanceBlind()
    {
        if (Phase == GamePhase.SmallBlind) { Phase = GamePhase.BigBlind; }
        else if (Phase == GamePhase.BigBlind) { Phase = GamePhase.BossBlind; }
        else { Phase = GamePhase.SmallBlind; Ante = Ante + 1; }
        BlindGoal = 20 * Ante * ((int)Phase + 1);
    }

    public void DrawHand()
    {
        HandManager.ClearHand();
        for (int i = 0; i < HandSize; i++)
        {
            Card c = DeckManager.Draw();
            if (c != null) { HandManager.AddCard(c); }
        }
        HandManager.SortHand();
    }

    private void RefreshHandUI()
    {
        for (int i = 0; i < cardUIObjects.Count; i++) { Destroy(cardUIObjects[i]); }
        cardUIObjects.Clear();
        List<Card> currentHand = HandManager.GetHand();
        for (int i = 0; i < currentHand.Count; i++)
        {
            GameObject ui = Instantiate(cardUIPrefab, handUIParent);
            cardUIObjects.Add(ui);
            ui.GetComponent<CardUI>().cardImage.sprite = Resources.Load<Sprite>("Cards/" + currentHand[i].GetSuit() + "_" + currentHand[i].GetValue());
        }
    }

    private void ToggleSelection(int index)
    {
        if (index >= cardUIObjects.Count) { return; }
        RectTransform rt = cardUIObjects[index].GetComponent<RectTransform>();
        if (selectedIndices.Contains(index))
        {
            selectedIndices.Remove(index);
            rt.anchoredPosition = new Vector2(0, 0);
        }
        else
        {
            selectedIndices.Add(index);
            rt.anchoredPosition = new Vector2(0, liftAmount);
        }
    }

    private Card GenerateRandomCard()
    {
        string[] s = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] v = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        return new Card(s[Random.Range(0, 4)], v[Random.Range(0, 13)]);
    }

    public void SetIsInGame(bool v) { IsInGame = v; }
    public void SaveGame() { SaveSystem.SaveGame(this); }
    public int GetMoney() { return EconomyManager.GetMoney(); }
    public int GetAnte() { return Ante; }
    public GamePhase GetPhase() { return Phase; }
    public int GetHandSize() { return HandSize; }
    public int GetHandsRemaining() { return HandsRemaining; }
    public int GetFinalScore() { return FinalScore; }
    public int GetBlindGoal() { return BlindGoal; }
    public List<Card> GetFullDeck() { return DeckManager.GetFullDeck(); }
    public List<Card> GetCurrentDeck() { return DeckManager.GetCurrentDeck(); }
    public List<Joker> GetActiveJokers() { return JokerManager.GetActiveJokers(); }
    public void LoadDeck(List<Card> f, List<Card> c) { DeckManager.LoadDecks(f, c); }
    public void LoadJokers(List<Joker> j) { JokerManager.LoadJokers(j); ScoreManager = new ScoreManager(JokerManager); }
}