using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class SpecialAction
{
    public static SpecialAction GetRandomSpecialAction()
    {
        SpecialAction action = new SpecialAction();
        action.Type = (SpecialActionType) Random.Range(1, 3);
        return action;
    }

    public static SpecialAction GetSpecialAction(SpecialActionType type)
    {
        SpecialAction action = new SpecialAction();
        action.Type = type;
        return action;
    }

    public SpecialActionType Type;

    protected SpecialAction()
    {

    }
}

public enum SpecialActionType
{
    None,
    CutOffCircuit,
    CutGasSource
}
