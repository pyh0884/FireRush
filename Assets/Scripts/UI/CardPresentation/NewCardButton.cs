using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewCardButton : MonoBehaviour
{
    public static event Action NewCardButtonSelected;

    public void GetNewCard()
    {
        DebugExtension.DebugText("Remaining Score is {0}, Total Fire power {1}", GameData.Instance.RemainingScore, GameData.Instance.TotalFirePower);
        GameManager.Instance.ReDeal();
        DebugExtension.DebugText("DEAL!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        DebugExtension.DebugText("Remaining Score is {0}, Total Fire power {1}", GameData.Instance.RemainingScore, GameData.Instance.TotalFirePower);

        if (NewCardButtonSelected != null)
        {
            NewCardButtonSelected();
        }

        //debug function
        //DebugGetNewCard();
    }

    #region Debug Sector
    /*
    //debug sector
    [SerializeField] int water;
    [SerializeField] int sand;
    [SerializeField] int foam;
    [SerializeField] SpecialActionType specialitem;
    [SerializeField] int slotIndex;
    [SerializeField] CardSlotManager cardSlotManager;
    [SerializeField] bool isCardChange;

    public void DebugGetNewCard()
    {
        ResourcePackage resourcePackage = new ResourcePackage(water, sand, foam);
        resourcePackage.SpecialAction = specialitem;

        if (isCardChange)
        {
            cardSlotManager.ChangeCard(slotIndex, resourcePackage);
        }
        else
        {
            cardSlotManager.SpawnCard(slotIndex, resourcePackage);
        }
        
    }
    */
    #endregion
}
