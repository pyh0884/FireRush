using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFire : Fire
{
    public ElectricFire(float level = 0, float enlargeSpeed = 0.1f):base(level, enlargeSpeed)
    {
        Type = FireType.Electric;
        EnlargeFactor = 1.1f;
        WaterExtinguishFactor = -1.5f;
        FoamExtinguishFactor = -1.5f;
        SandExtinguishFactor = 1.2f;
    }

    public override Fire SpecialActionHappened(SpecialActionType action)
    {
        if (action == SpecialActionType.CutOffCircuit)
        {
            Debug.Log("Circuit Cut!");

            return FireFactory.GetFireOfType(FireType.Normal, CurrentFireLevel, EnlargeSpeed);
            
        }

        return null;
    }

    public override bool ActionHappened(ResourceType resourceType, float amount)
    {
        switch(resourceType)
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
                return false;
            }
        }
        return true;
    }
}
