using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fire
{
    public const int MAX_FIRE_LEVEL = 5;

    public float CurrentFireLevel = 0;
    public float EnlargeSpeed = 0.1f;
    // Enlarge speed will multiply by this value which depends on fire's type
    public float EnlargeFactor = 1;

    public float WaterExtinguishFactor = 1;
    public float SandExtinguishFactor = 1;
    public float FoamExtinguishFactor = 1;

    public FireType Type;
    public bool Vanished = false;

    public int FireLevelInt
    {
        get { return Mathf.FloorToInt(CurrentFireLevel); }
    }

    public Fire(float level = 1, float enlargeSpeed = 0.1f)
    {
        Type = FireType.Normal;
        CurrentFireLevel = level;
        EnlargeSpeed = enlargeSpeed;
    }

    /// <summary>
    /// Special action happened
    /// </summary>
    /// <param name="action"></param>
    /// <returns>New Fire</returns>
    public virtual Fire SpecialActionHappened(SpecialActionType action)
    {
        return null;
    }

    /// <summary>
    /// Normal action happened
    /// </summary>
    /// <param name="usingResources">Resources for doing action</param>
    /// <returns></returns>
    public virtual bool ActionHappened(ResourceType resourceType, float amount)
    {
        switch (resourceType)
        {
            case ResourceType.Water:
            {
                CurrentFireLevel -= amount * WaterExtinguishFactor;
                return true;
            }

            case ResourceType.Sand:
            {
                CurrentFireLevel -= amount * SandExtinguishFactor;
                return true;
            }

            case ResourceType.Foam:
            {
                CurrentFireLevel -= amount * FoamExtinguishFactor;
                return true;
            }
        }
        return true;
    }
}

public enum FireType
{
    Normal,
    Electric,
    Gas,
    Oil
}
