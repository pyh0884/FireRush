using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FireInfo
{
    public float HappenTime = 0;
    public FireType Type = FireType.Normal;

    public int StartLevel = 1;
    [Range(0, 5)]public float EnlargeSpeed = 0.1f;

    public FireInfo()
    {

    }

    public FireInfo(FireInfo original)
    {
        HappenTime = original.HappenTime;
        Type = original.Type;

        StartLevel = original.StartLevel;
        EnlargeSpeed = original.EnlargeSpeed;
    }
}
