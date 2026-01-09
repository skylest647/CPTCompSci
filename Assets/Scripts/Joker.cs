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
    protected int cost;

    public string getCost()
    {
        return cost;
    }
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

// using System.Collections.Generic;
// using UnityEngine;

// public class TEMPLATE_JokerName : Joker
// {
//     public TEMPLATE_JokerName()
//     {
//         Name = "Joker Name Here";
//         Description = "Describe what this Joker does";
//         EffectType = JokerEffectType.Chips; 
//         // or JokerEffectType.Multiplier
//     }

//     // Only override this if EffectType == Chips
//     public override int GetBonusChips(HandResult handResult, List<Card> hand)
//     {
//         // Example condition
//         // if (handResult.Type == HandType.Pair)
//         //     return 15;

//         return 0;
//     }

//     // Only override this if EffectType == Multiplier
//     public override float GetBonusMultiplier(HandResult handResult, List<Card> hand)
//     {
//         // Example condition
//         // if (handResult.Type == HandType.Flush)
//         //     return 2f;

//         return 0f;
//     }
// }