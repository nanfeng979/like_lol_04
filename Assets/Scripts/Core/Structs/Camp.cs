
using System;

[Serializable]
public struct Camp
{
    public CampType Type;

}

[Serializable]
public enum CampType
{
    Neutral,
    Blue,
    Red
}