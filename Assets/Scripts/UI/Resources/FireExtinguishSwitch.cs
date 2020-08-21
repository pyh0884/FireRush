using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireExtinguishSwitch : MonoBehaviour
{
    public static event Action FireExtinguishSwitchSelected;
    [SerializeField] ResourceSelection resourceSelection;


    public void Execute()
    {
        CalculateResource();
        
        if (FireExtinguishSwitchSelected != null)//active event
        {
            FireExtinguishSwitchSelected();
        }
        //DebugExtension.DebugText("");
    }

    public ResourceType CardAttributeToResourceType(CardAttribute.ITEM_TYPE cardAttributeType)
    {
        switch (cardAttributeType)
        {
            case CardAttribute.ITEM_TYPE.WATER:
                return ResourceType.Water;

            case CardAttribute.ITEM_TYPE.SAND:
                return ResourceType.Sand;

            case CardAttribute.ITEM_TYPE.FOAM:
                return ResourceType.Foam;
        }
        DebugExtension.DebugText("Wrong CardAttribute Type For Fire Extinguish exection");
        return ResourceType.Water;
    }

    public void CalculateResource() //check which resource is selected
    {
        CardAttribute.ITEM_TYPE selectedResource = resourceSelection.currentResource;   
        DebugExtension.DebugText("Fire Extinguishi Executed! Using {0}", selectedResource);

        //send resource message to other function
        GameManager.Instance.UseResources(CardAttributeToResourceType(selectedResource));
    }
}
