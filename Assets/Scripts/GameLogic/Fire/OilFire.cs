using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilFire : Fire
{
    public OilFire(float level = 0, float enlargeSpeed = 0.1f) : base(level, enlargeSpeed)
    {
        Type = FireType.Oil;
        EnlargeFactor = 1.2f;
        WaterExtinguishFactor = -1;
    }

    public override Fire SpecialActionHappened(SpecialActionType action)
    {
        return base.SpecialActionHappened(action);
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
