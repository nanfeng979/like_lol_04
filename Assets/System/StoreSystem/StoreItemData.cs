using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoreItemData
{
    public string Name;
    public string Category;
    public int Cost;
    public StoreItemDataAttribute Attribute;
    public string[] Filter1;
    public string[] Filter2;
}

[System.Serializable]
public class StoreItemDataAttribute
{
    public int AttackDamage;
    public int CriticalStrikeChance;
    public int CriticalStrikeDamage;
}
