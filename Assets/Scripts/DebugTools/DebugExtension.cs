using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugExtension
{
    public static void DebugText(string text, params object[] format)
    {
        Debug.Log(string.Format(text, format));
    }
}
