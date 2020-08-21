using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Data/Level")]
public class Level : ScriptableObject
{
    [Tooltip("All fires will happen in game")]
    public List<FireInfo> FireInfos;

    [Tooltip("Max fire power of all fires")]
    public int MaxFirePower;

    [Tooltip("Interval between auto deals")]
    public float DealInterval = 3;

    [Tooltip("Resource score when starting")]
    public float InitialScore = 200;

    [Tooltip("Max resource when game starts")]
    public ResourcePackage MaxResources;

    [Tooltip("Initial resources")]
    public ResourcePackage InitialResources;

    [Tooltip("Actions need to be triggerd to win the game")]
    public List<SpecialActionType> ActionNeeded;

    [Tooltip("How many cards will be put on board at same time")]
    public int CardsOnBoard = 4;

    [Tooltip("Heat value add every time player make a new deal by hand")]
    public float HeatAddByHand = 0.1f;
}
