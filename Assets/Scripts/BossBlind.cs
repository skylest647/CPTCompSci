using UnityEngine;
using System.Collections.Generic;

public abstract class BossBlind
{
    protected string name;
    protected string description;

    public string GetName() { return name; }
    public string GetDescription() { return description; }

    // Override these methods to implement boss effects
    public virtual bool CanPlayHand(HandType handType, int handSize) { return true; }
    public virtual bool IsCardDebuffed(Card card, int position) { return false; }
    public virtual int ModifyScore(int score) { return score; }
    public virtual void OnBlindStart() { }
    public virtual void OnHandPlayed() { }
}

// BOSS BLIND IMPLEMENTATIONS

public class BossTheWall : BossBlind
{
    public BossTheWall()
    {
        name = "The Wall";
        description = "Extra large blind (2.5x base score)";
    }

    // This would be handled in BlindManager score calculation
}

public class BossTheFlint : BossBlind
{
    public BossTheFlint()
    {
        name = "The Flint";
        description = "Base Chips and Mult are halved";
    }

    public override int ModifyScore(int score)
    {
        return score / 2;
    }
}

public class BossTheGoad : BossBlind
{
    public BossTheGoad()
    {
        name = "The Goad";
        description = "All Spades are debuffed";
    }

    public override bool IsCardDebuffed(Card card, int position)
    {
        return card.GetSuit() == "Spades";
    }
}

public class BossTheHead : BossBlind
{
    public BossTheHead()
    {
        name = "The Head";
        description = "All Hearts are debuffed";
    }

    public override bool IsCardDebuffed(Card card, int position)
    {
        return card.GetSuit() == "Hearts";
    }
}

public class BossPsychic : BossBlind
{
    public BossPsychic()
    {
        name = "The Psychic";
        description = "Must play 5 cards";
    }

    public override bool CanPlayHand(HandType handType, int handSize)
    {
        return handSize == 5;
    }
}

public class BossTheClub : BossBlind
{
    public BossTheClub()
    {
        name = "The Club";
        description = "All Clubs are debuffed";
    }

    public override bool IsCardDebuffed(Card card, int position)
    {
        return card.GetSuit() == "Clubs";
    }
}

public class BossTheWindow : BossBlind
{
    public BossTheWindow()
    {
        name = "The Window";
        description = "All Diamond cards are debuffed";
    }

    public override bool IsCardDebuffed(Card card, int position)
    {
        return card.GetSuit() == "Diamonds";
    }
}

public class BossTheNeedle : BossBlind
{
    public BossTheNeedle()
    {
        name = "The Needle";
        description = "Can only play 1 hand";
    }

    // Would be handled in GameManager - set hands to 1
}