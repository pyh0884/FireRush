using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeUI : MonoBehaviour
{
    public static FakeUI Instance { get; protected set; }

    public Text[] Cards;
    public Text GetItemNumber;

    public Text NextDealTime;
    public Text GameTime;

    public Text ResourcesInHand;
    public Text SelectedResource;
    public Text SpecialActionInHand;
    public Text AllFires;

    public Text FirePower;
    public Text RemainScore;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateCardInSlot(int index, ResourcePackage card)
    {
        Cards[index].text = card.SpecialAction == SpecialActionType.None
            ? string.Format("Water:{0} \nSand:{1} \nFoam:{2}", card.Water, card.Sand, card.Foam)
            : "Special:" + card.SpecialAction.ToString("G");
    }

    public void UpdateUI()
    {
        NextDealTime.text = "Next Deal: " + GameManager.Instance.DealTimer.ToString("F2") + "/" + GameData.Instance.DealInterval;
        GameTime.text = "Game Time: " + GameManager.Instance.GameTimer.ToString("F2");

        ResourcesInHand.text = string.Format("Water:{0} \nSand:{1} \nFoam:{2}",
            GameData.Instance.AvailableResources.Water + "/" + GameData.Instance.MaxResources.Water, 
            GameData.Instance.AvailableResources.Sand + "/" + GameData.Instance.MaxResources.Sand,
            GameData.Instance.AvailableResources.Foam + "/" + GameData.Instance.MaxResources.Foam);
        SelectedResource.text = ResourcesSelector.Instance.SelectedResource.ToString("G");

        string allFires = "";
        foreach (Fire fire in GameData.Instance.OnboardFires)
        {
            allFires += String.Format("Cur:{0} \nEnl:{1} \nType:{2}\n\n",
                fire.CurrentFireLevel.ToString("F2"), fire.EnlargeSpeed, fire.Type.ToString("G"));
        }
        AllFires.text = allFires;

        FirePower.text = "Total Fire Power: " + GameData.Instance.TotalFirePower;
        RemainScore.text = "Remain Score: " + GameData.Instance.RemainingScore;
        GetItemNumber.text = GameManager.Instance.GetItemNumber.ToString();
        SpecialActionInHand.text = GameData.Instance.AvailableResources.SpecialAction.ToString("G");
    }
}
