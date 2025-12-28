using UnityEngine;
using System.Collections.Generic;

public enum JokerEffectType
{
    Chips,
    Multiplier
}
public class Joker
{
    //protected so that inherited classes can work :p
    protected string Name;
    protected string Description;
    protected JokerEffectType EffectType;

    public string GetName()
    {
        return Name;
    }

    public string GetDescription()
    {
        return Description;
    }

    public JokerEffectType GetEffectType()
    {
        return EffectType;
    }

    public virtual int GetBonusChips(HandResult handResult, List<Card> hand)
    {
        return 0;
    }

    public virtual float GetBonusMultiplier(HandResult handResult, List<Card> hand)
    {
        return 0f;
    }
}