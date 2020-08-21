using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FireFactory
{
    public static Fire GetFireOfType(FireType type, float level = 1, float enlargeSpeed = 0.1f)
    {
        switch (type)
        {
            case FireType.Normal:
            {
                return new Fire(level, enlargeSpeed);
            }

            case FireType.Electric:
            {
                return new ElectricFire(level, enlargeSpeed);
            }

            case FireType.Gas:
            {
                return new GasFire(level, enlargeSpeed);
            }

            case FireType.Oil:
            {
                return new OilFire(level, enlargeSpeed);
            }
        }

        throw new Exception("No such fire type: " + type.ToString("G"));
    }
}
