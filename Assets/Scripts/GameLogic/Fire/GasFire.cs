using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasFire : Fire
{
    public GasFire(float level = 0, float enlargeSpeed = 0.1f) : base(level, enlargeSpeed)
    {
        Type = FireType.Gas;
        EnlargeFactor = 1.4f;
        WaterExtinguishFactor = 0;
        FoamExtinguishFactor = 0.2f;
    }

    public override Fire SpecialActionHappened(SpecialActionType action)
    {
        if (action == SpecialActionType.CutGasSource)
        {
            Debug.Log("Gas Cut!");
            return FireFactory.GetFireOfType(FireType.Normal, CurrentFireLevel, EnlargeSpeed);
        }

        return null;
    }

    public override bool ActionHappened(ResourceType resourceType, float amount)
    {
        switch (resourceType)
        {
            case ResourceType.Water:
            {
                CurrentFireLevel -= amount * WaterExtinguishFactor;
                return false;
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
