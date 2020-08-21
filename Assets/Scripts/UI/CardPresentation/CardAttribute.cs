using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// instantiate this class in the scene
/// change the instance of this class to set the card icon or other settings
/// </summary>
public class CardAttribute : MonoBehaviour
{
    public static CardAttribute _instance;

    public enum ITEM_TYPE
    {
        WATER,
        SAND,
        FOAM,
        SPECIAL
    }

    public Sprite[] itemIcon; //each icon refer to the item type
    public Sprite[] specialActionIcon; //each icon refer to a special item

    private void Awake()
    {
        _instance = this;
    }
}
