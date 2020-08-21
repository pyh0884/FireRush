using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A vector of all the resources count
/// </summary>
[System.Serializable]
public class ResourcePackage
{
    public int Water = 0;
    public int Sand = 0;
    public int Foam = 0;

    public SpecialActionType SpecialAction = SpecialActionType.None;

    public int Sum
    {
        get { return (Water + Sand + Foam); }
    }

    public ResourcePackage()
    {

    }

    public ResourcePackage(int water, int sand, int foam)
    {
        Water = water;
        Sand = sand;
        Foam = foam;
    }

    public ResourcePackage(SpecialActionType specialActionType)
    {
        SpecialAction = specialActionType;
    }
}

public enum ResourceType
{
    Water,
    Sand,
    Foam
}
