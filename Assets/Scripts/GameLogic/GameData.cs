using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data need to be updated every frame
/// </summary>
public class GameData
{
    public static GameData Instance { get; protected set; }

    public ResourcePackage AvailableResources; // All the resources you've got

    public ResourcePackage MaxResources; // Upper limit of all resources

    public List<FireInfo> WillHappenFires = new List<FireInfo>(); // Fires will happen in game
    public List<Fire> OnboardFires = new List<Fire>(); // All fires player need to put out

    public float DealInterval = 0;

    public float InitialScore = 200;
    public float RemianingScoreFloat = 200;

    // Sum of all fire's power
    public float TotalFirePower
    {
        get
        {
            float power = 0;
            foreach (Fire fire in OnboardFires)
            {
                if (!fire.Vanished)
                {
                    power += fire.CurrentFireLevel;
                }
            }
            return power;
        }
    }
    public int MaxFirePower;

    // if remaining score turned to 0, player will lose the game
    public int RemainingScore
    {
        get { return Mathf.CeilToInt(RemianingScoreFloat); }
    }

    public GameData()
    {
        AvailableResources = new ResourcePackage();
        Instance = this;
    }

    public void IncreaseFirePower()
    {
        foreach (Fire fire in OnboardFires)
        {
            if (!fire.Vanished)
            {
                fire.CurrentFireLevel += fire.EnlargeSpeed * fire.EnlargeFactor * Time.deltaTime;
            }
        }
    }

    public void GetSpecialAction(SpecialActionType type)
    {
        AvailableResources.SpecialAction = type;
    }

    public void AddResources(bool water, bool sand, bool foam)
    {
        if (water && AvailableResources.Water < MaxResources.Water)
        {
            AvailableResources.Water++;
        }

        if (sand && AvailableResources.Sand < MaxResources.Sand)
        {
            AvailableResources.Sand++;
        }

        if (foam && AvailableResources.Foam < MaxResources.Foam)
        {
            AvailableResources.Foam++;
        }
    }

    public void TriggerSpecialAction()
    {
        if (AvailableResources.SpecialAction != SpecialActionType.None)
        {
            List<Fire> needRemoveFire = new List<Fire>();
            List<Fire> needAddFire = new List<Fire>();
            List<int> originalFireIndex = new List<int>();

            foreach (Fire fire in OnboardFires)
            {
                if (fire.Vanished)
                {
                    continue;
                }

                Fire newFire = fire.SpecialActionHappened(AvailableResources.SpecialAction);
                if (newFire != null)
                {
                    originalFireIndex.Add(OnboardFires.IndexOf(fire));
                    needRemoveFire.Add(fire);
                    needAddFire.Add(newFire);
                }
            }

            for(int i = 0; i < needAddFire.Count; i++)
            {
                OnboardFires.Remove(needRemoveFire[i]);
                OnboardFires.Insert(originalFireIndex[i], needAddFire[i]);
            }
        }
    }

    public void UseResource(ResourceType type)
    {
        float amount = 0;

        switch (type)
        {
            case ResourceType.Water:
            {
                amount = (float) AvailableResources.Water / OnboardFires.Count;
                AvailableResources.Water = 0;
                break;
            }

            case ResourceType.Sand:
            {
                amount = (float)AvailableResources.Sand / OnboardFires.Count;
                AvailableResources.Sand = 0;
                break;
            }

            case ResourceType.Foam:
            {
                amount = (float)AvailableResources.Foam / OnboardFires.Count;
                AvailableResources.Foam = 0;
                break;
            }
        }

        for (int i = 0; i < OnboardFires.Count; i++)
        {
            Fire fire = OnboardFires[i];

            if (fire.Vanished)
            {
                continue;
            }

            fire.ActionHappened(type, amount);

            if (fire.CurrentFireLevel <= 0)
            {
                fire.Vanished = true;
                //OnboardFires.RemoveAt(i);
                //i--;
            }
        }
    }

    public void AddHeat(float heat)
    {
        int validFireCount = 0;

        for (int i = 0; i < OnboardFires.Count; i++)
        {
            if (!OnboardFires[i].Vanished)
            {
                validFireCount++;
            }
        }

        foreach (Fire fire in OnboardFires)
        {
            fire.CurrentFireLevel += heat / validFireCount;
        }
    }
}


