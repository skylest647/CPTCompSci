using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;

public class AllJokers
{
    private List<Joker> jokers;

    public AllJokers()
    {
        jokers = new List<Joker>
        {
            new JokerJoker(),

            // Suit-based jokers
            new GreedyJoker(),
            new LustyJoker(),
            new WrathfulJoker(),
            new GluttonousJoker(),

            // Hand multiplier jokers
            new JollyJoker(),
            new ZanyJoker(),
            new MadJoker(),
            new CrazyJoker(),
            new DevotedJoker(),

            // Hand chip jokers
            new SlyJoker(),
            new WilyJoker(),
            new CleverJoker(),
            new DeviousJoker(),
            new CraftyJoker(),

            // Special condition jokers
            new HalfJoker(),
            new StuntmanJoker(),
            new RaisedFist(),
            new ScaredFace(),
            new AbstractJoker()
        };
    }

    public Joker GetRandomJoker()
    {
        int index = UnityEngine.Random.Range(0, jokers.Count);
        Joker template = jokers[index];
        return System.Activator.CreateInstance(template.GetType()) as Joker;
    }
}
public class JokerJoker : Joker
{
    public JokerJoker()
    {
        Name = "Joker";
        Description = "+4 Multiplier";
        EffectType = JokerEffectType.Multiplier;
    }

    public override float GetBonusMultiplier(HandResult handResult, List<Card> hand)
    {
        return 4f;
    }
}

public class GreedyJoker : Joker
{
    public GreedyJoker()
    {
        Name = "Greedy Joker";
        Description = "Played cards with Diamond suit give +3 Multiplier when scored";
        EffectType = JokerEffectType.Multiplier;
    }

    public override float GetBonusMultiplier(HandResult handResult, List<Card> hand)
    {
        int diamondCount = 0;
        foreach (Card card in hand)
        {
            if (card.GetSuit() == "Diamonds")
                diamondCount++;
        }
        return diamondCount * 3f;
    }
}

public class LustyJoker : Joker
{
    public LustyJoker()
    {
        Name = "Lusty Joker";
        Description = "Played cards with Heart suit give +3 Multiplier when scored";
        EffectType = JokerEffectType.Multiplier;
    }

    public override float GetBonusMultiplier(HandResult handResult, List<Card> hand)
    {
        int heartCount = 0;
        foreach (Card card in hand)
        {
            if (card.GetSuit() == "Hearts")
                heartCount++;
        }
        return heartCount * 3f;
    }
}

public class WrathfulJoker : Joker
{
    public WrathfulJoker()
    {
        Name = "Wrathful Joker";
        Description = "Played cards with Spade suit give +3 Multiplier when scored";
        EffectType = JokerEffectType.Multiplier;
    }

    public override float GetBonusMultiplier(HandResult handResult, List<Card> hand)
    {
        int spadeCount = 0;
        foreach (Card card in hand)
        {
            if (card.GetSuit() == "Spades")
                spadeCount++;
        }
        return spadeCount * 3f;
    }
}

public class GluttonousJoker : Joker
{
    public GluttonousJoker()
    {
        Name = "Gluttonous Joker";
        Description = "Played cards with Club suit give +3 Multiplier when scored";
        EffectType = JokerEffectType.Multiplier;
    }

    public override float GetBonusMultiplier(HandResult handResult, List<Card> hand)
    {
        int clubCount = 0;
        foreach (Card card in hand)
        {
            if (card.GetSuit() == "Clubs")
                clubCount++;
        }
        return clubCount * 3f;
    }
}

public class JollyJoker : Joker
{
    public JollyJoker()
    {
        Name = "Jolly Joker";
        Description = "+8 Multiplier if played hand contains a Pair";
        EffectType = JokerEffectType.Multiplier;
    }

    public override float GetBonusMultiplier(HandResult handResult, List<Card> hand)
    {
        if (handResult.Type == HandType.Pair || handResult.Type == HandType.TwoPair || 
            handResult.Type == HandType.FullHouse)
            return 8f;
        return 0f;
    }
}

public class ZanyJoker : Joker
{
    public ZanyJoker()
    {
        Name = "Zany Joker";
        Description = "+12 Multiplier if played hand contains a Three of a Kind";
        EffectType = JokerEffectType.Multiplier;
    }

    public override float GetBonusMultiplier(HandResult handResult, List<Card> hand)
    {
        if (handResult.Type == HandType.ThreeOfAKind || handResult.Type == HandType.FullHouse)
            return 12f;
        return 0f;
    }
}

public class MadJoker : Joker
{
    public MadJoker()
    {
        Name = "Mad Joker";
        Description = "+10 Multiplier if played hand contains a Two Pair";
        EffectType = JokerEffectType.Multiplier;
    }

    public override float GetBonusMultiplier(HandResult handResult, List<Card> hand)
    {
        if (handResult.Type == HandType.TwoPair)
            return 10f;
        return 0f;
    }
}

public class CrazyJoker : Joker
{
    public CrazyJoker()
    {
        Name = "Crazy Joker";
        Description = "+12 Multiplier if played hand contains a Straight";
        EffectType = JokerEffectType.Multiplier;
    }

    public override float GetBonusMultiplier(HandResult handResult, List<Card> hand)
    {
        if (handResult.Type == HandType.Straight || handResult.Type == HandType.StraightFlush)
            return 12f;
        return 0f;
    }
}

public class DevotedJoker : Joker
{
    public DevotedJoker()
    {
        Name = "Devoted Joker";
        Description = "+10 Multiplier if played hand contains a Flush";
        EffectType = JokerEffectType.Multiplier;
    }

    public override float GetBonusMultiplier(HandResult handResult, List<Card> hand)
    {
        if (handResult.Type == HandType.Flush || handResult.Type == HandType.StraightFlush)
            return 10f;
        return 0f;
    }
}

public class SlyJoker : Joker
{
    public SlyJoker()
    {
        Name = "Sly Joker";
        Description = "+50 Chips if played hand contains a Pair";
        EffectType = JokerEffectType.Chips;
    }

    public override int GetBonusChips(HandResult handResult, List<Card> hand)
    {
        if (handResult.Type == HandType.Pair || handResult.Type == HandType.TwoPair || 
            handResult.Type == HandType.FullHouse)
            return 50;
        return 0;
    }
}

public class WilyJoker : Joker
{
    public WilyJoker()
    {
        Name = "Wily Joker";
        Description = "+100 Chips if played hand contains a Three of a Kind";
        EffectType = JokerEffectType.Chips;
    }

    public override int GetBonusChips(HandResult handResult, List<Card> hand)
    {
        if (handResult.Type == HandType.ThreeOfAKind || handResult.Type == HandType.FullHouse)
            return 100;
        return 0;
    }
}

public class CleverJoker : Joker
{
    public CleverJoker()
    {
        Name = "Clever Joker";
        Description = "+80 Chips if played hand contains a Two Pair";
        EffectType = JokerEffectType.Chips;
    }

    public override int GetBonusChips(HandResult handResult, List<Card> hand)
    {
        if (handResult.Type == HandType.TwoPair)
            return 80;
        return 0;
    }
}

public class DeviousJoker : Joker
{
    public DeviousJoker()
    {
        Name = "Devious Joker";
        Description = "+100 Chips if played hand contains a Straight";
        EffectType = JokerEffectType.Chips;
    }

    public override int GetBonusChips(HandResult handResult, List<Card> hand)
    {
        if (handResult.Type == HandType.Straight || handResult.Type == HandType.StraightFlush)
            return 100;
        return 0;
    }
}

public class CraftyJoker : Joker
{
    public CraftyJoker()
    {
        Name = "Crafty Joker";
        Description = "+80 Chips if played hand contains a Flush";
        EffectType = JokerEffectType.Chips;
    }

    public override int GetBonusChips(HandResult handResult, List<Card> hand)
    {
        if (handResult.Type == HandType.Flush || handResult.Type == HandType.StraightFlush)
            return 80;
        return 0;
    }
}

public class HalfJoker : Joker
{
    public HalfJoker()
    {
        Name = "Half Joker";
        Description = "+20 Multiplier if played hand contains 3 or fewer cards";
        EffectType = JokerEffectType.Multiplier;
    }

    public override float GetBonusMultiplier(HandResult handResult, List<Card> hand)
    {
        if (hand.Count <= 3)
            return 20f;
        return 0f;
    }
}

public class StuntmanJoker : Joker
{
    public StuntmanJoker()
    {
        Name = "Stuntman";
        Description = "+250 Chips, -2 hand size";
        EffectType = JokerEffectType.Chips;
    }

    public override int GetBonusChips(HandResult handResult, List<Card> hand)
    {
        return 250;
    }
}

public class RaisedFist : Joker
{
    public RaisedFist()
    {
        Name = "Raised Fist";
        Description = "+25 Multiplier, doubles if you play your lowest ranked card";
        EffectType = JokerEffectType.Multiplier;
    }

    public override float GetBonusMultiplier(HandResult handResult, List<Card> hand)
    {
        return 25f;
    }
}

public class ScaredFace : Joker
{
    public ScaredFace()
    {
        Name = "Scared Face";
        Description = "Played face cards give +30 Chips when scored";
        EffectType = JokerEffectType.Chips;
    }

    public override int GetBonusChips(HandResult handResult, List<Card> hand)
    {
        int faceCount = 0;
        foreach (Card card in hand)
        {
            string value = card.GetValue();
            if (value == "J" || value == "Q" || value == "K")
                faceCount++;
        }
        return faceCount * 30;
    }
}

public class AbstractJoker : Joker
{
    public AbstractJoker()
    {
        Name = "Abstract Joker";
        Description = "+3 Multiplier for each Joker card (currently always +3)";
        EffectType = JokerEffectType.Multiplier;
    }

    public override float GetBonusMultiplier(HandResult handResult, List<Card> hand)
    {
        return 3f;
        //not done
    }
}