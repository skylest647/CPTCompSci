using UnityEngine;
using System.Collections.Generic;

public enum BlindType
{
    SmallBlind,
    BigBlind,
    BossBlind
}

public class BlindManager
{
    private BlindType currentBlind;
    private int ante;
    private BossBlind currentBoss;
    private List<BossBlind> availableBosses;
    private List<BossBlind> usedBosses;
    
    private const int BASE_SMALL_BLIND = 300;
    private const int BASE_BIG_BLIND = 450;
    private const int BASE_BOSS_BLIND = 600;

    public BlindManager()
    {
        currentBlind = BlindType.SmallBlind;
        ante = 1;
        InitializeBosses();
    }

    private void InitializeBosses()
    {
        availableBosses = new List<BossBlind>
        {
            new BossTheWall(),
            new BossTheFlint(),
            new BossTheGoad(),
            new BossTheHead(),
            new BossPsychic(),
            new BossTheClub(),
            new BossTheWindow(),
            new BossTheNeedle()
        };
        
        usedBosses = new List<BossBlind>();
    }

    public int GetBlindScore()
    {
        switch (currentBlind)
        {
            case BlindType.SmallBlind:
                return CalculateBlindScore(BASE_SMALL_BLIND);
            case BlindType.BigBlind:
                return CalculateBlindScore(BASE_BIG_BLIND);
            case BlindType.BossBlind:
                return CalculateBlindScore(BASE_BOSS_BLIND);
            default:
                return 0;
        }
    }

    private int CalculateBlindScore(int baseScore)
    {
        // Score scaling: increases exponentially with ante
        // Ante 1: 1x, Ante 2: 1.6x, Ante 3: 2.4x, Ante 4: 3.4x, etc.
        float multiplier = 1f + (ante - 1) * 0.6f + Mathf.Pow(ante - 1, 1.6f) * 0.1f;
        return Mathf.RoundToInt(baseScore * multiplier);
    }

    public void AdvanceBlind()
    {
        switch (currentBlind)
        {
            case BlindType.SmallBlind:
                currentBlind = BlindType.BigBlind;
                break;
            case BlindType.BigBlind:
                currentBlind = BlindType.BossBlind;
                SelectRandomBoss();
                break;
            case BlindType.BossBlind:
                currentBlind = BlindType.SmallBlind;
                ante++;
                currentBoss = null;
                break;
        }
    }

    private void SelectRandomBoss()
    {
        // If all bosses have been used, reset the pool
        if (availableBosses.Count == 0)
        {
            availableBosses.AddRange(usedBosses);
            usedBosses.Clear();
        }

        // Select random boss
        int index = Random.Range(0, availableBosses.Count);
        currentBoss = availableBosses[index];
        
        // Move to used pool
        usedBosses.Add(currentBoss);
        availableBosses.RemoveAt(index);
    }

    public BlindType GetCurrentBlind()
    {
        return currentBlind;
    }

    public int GetAnte()
    {
        return ante;
    }

    public BossBlind GetCurrentBoss()
    {
        return currentBoss;
    }

    public bool IsBossBlind()
    {
        return currentBlind == BlindType.BossBlind;
    }

    public string GetBlindName()
    {
        if (currentBlind == BlindType.BossBlind && currentBoss != null)
            return currentBoss.GetName();
        
        return currentBlind.ToString();
    }

    public string GetBlindDescription()
    {
        if (currentBlind == BlindType.BossBlind && currentBoss != null)
            return currentBoss.GetDescription();
        
        return "No special effects";
    }

    public int GetReward()
    {
        switch (currentBlind)
        {
            case BlindType.SmallBlind:
                return 3 + ante;
            case BlindType.BigBlind:
                return 4 + ante;
            case BlindType.BossBlind:
                return 5 + ante * 2;
            default:
                return 0;
        }
    }

    // Boss Blind effect methods - called by GameManager
    public bool CanPlayHand(HandType handType, int handSize)
    {
        if (currentBoss != null)
            return currentBoss.CanPlayHand(handType, handSize);
        return true;
    }

    public bool IsCardDebuffed(Card card, int position)
    {
        if (currentBoss != null)
            return currentBoss.IsCardDebuffed(card, position);
        return false;
    }

    public int ModifyScore(int score)
    {
        if (currentBoss != null)
            return currentBoss.ModifyScore(score);
        return score;
    }

    public void OnBlindStart()
    {
        if (currentBoss != null)
            currentBoss.OnBlindStart();
    }

    public void OnHandPlayed()
    {
        if (currentBoss != null)
            currentBoss.OnHandPlayed();
    }

    // Save/Load
    public void LoadState(int ante, BlindType blind)
    {
        this.ante = ante;
        this.currentBlind = blind;
        
        if (blind == BlindType.BossBlind)
            SelectRandomBoss();
    }
}