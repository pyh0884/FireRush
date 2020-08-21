using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePresentation : MonoBehaviour
{
    [SerializeField] ResourceBarControl[] controlers;

    public void InitiateResourceBar(CardAttribute.ITEM_TYPE resourceType, int maxValue)
    {
        controlers[(int)resourceType].BarInitialize(maxValue);
    }

    public void ResetResourceBar(CardAttribute.ITEM_TYPE resourceType)
    {
        controlers[(int)resourceType].ResetBar();
    }

    public void ShowResource(CardAttribute.ITEM_TYPE resourceType, int resourceValue)
    {
        controlers[(int)resourceType].RefreshBar(resourceValue);
    }

    public void RefreshResource()
    {
        ShowResource(CardAttribute.ITEM_TYPE.WATER, GameData.Instance.AvailableResources.Water);
        ShowResource(CardAttribute.ITEM_TYPE.SAND, GameData.Instance.AvailableResources.Sand);
        ShowResource(CardAttribute.ITEM_TYPE.FOAM, GameData.Instance.AvailableResources.Foam);
    }

    public void UIInitiate()
    {
        UIClean();

        InitiateResourceBar(CardAttribute.ITEM_TYPE.WATER, GameData.Instance.MaxResources.Water);
        InitiateResourceBar(CardAttribute.ITEM_TYPE.SAND, GameData.Instance.MaxResources.Sand);
        InitiateResourceBar(CardAttribute.ITEM_TYPE.FOAM, GameData.Instance.MaxResources.Foam);
    }

    public void UIClean()
    {
        ResetResourceBar(CardAttribute.ITEM_TYPE.WATER);
        ResetResourceBar(CardAttribute.ITEM_TYPE.SAND);
        ResetResourceBar(CardAttribute.ITEM_TYPE.FOAM);
    }

    private void Start()
    {
        GameManager.UIUpdateAction += RefreshResource;
        GameManager.UIInitiate += UIInitiate;
    }

    private void OnDestroy()
    {
        GameManager.UIUpdateAction -= RefreshResource;
        GameManager.UIInitiate -= UIInitiate;
    }
}
