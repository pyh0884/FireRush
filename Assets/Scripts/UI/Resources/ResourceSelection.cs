using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceSelection : MonoBehaviour
{
    [SerializeField] CardAttribute.ITEM_TYPE _currentResource;

    public static event Action ResourceSelectionChange;

    public CardAttribute.ITEM_TYPE currentResource
    {
        get
        {
            return _currentResource;
        }
    }

    public void SelectResource(int itemType)
    {
        _currentResource = (CardAttribute.ITEM_TYPE)itemType;

        if (ResourceSelectionChange != null)
        {
            ResourceSelectionChange();
        }

        DebugExtension.DebugText("The selected resource change to {0}", currentResource);
    }
}
