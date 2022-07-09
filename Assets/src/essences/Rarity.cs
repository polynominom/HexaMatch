using System;
using System.Collections.Generic;
using Unity.Collections;
public class Rarity
{
    private Dictionary<EssenceType, RarityType> essenceRarityMap;

    public Rarity()
    {
        essenceRarityMap = new Dictionary<EssenceType, RarityType>
        {
            { EssenceType.green,  RarityType.common },
            { EssenceType.red,    RarityType.common },
            { EssenceType.yellow, RarityType.common },
            { EssenceType.blue,   RarityType.common },
            { EssenceType.white,  RarityType.uncommon },
            { EssenceType.black,  RarityType.uncommon },
            { EssenceType.cosmic, RarityType.rare }
        };
    }

    public Dictionary<EssenceType, RarityType> GetEssenceRarityMap()
    {
        return essenceRarityMap;
    }
}
