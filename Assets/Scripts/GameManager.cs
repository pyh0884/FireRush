using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; protected set; }

    [Header("Levels")] public List<Level> Levels;
    [Header("Settings")]
    public Vector2Int GetItemNumberRange;

    [Header("Info")]
    public int CurrentLevel = 1;

    public GameData GameData;

    public ResourcePackage[] OnboardCards;
    public int GetItemNumber = 5;
    public int NextCardIndex = 0;
    public float DealByHandHeatAddValue = 0;

    [Header("UI Module")]
    [SerializeField] CardSlotManager cardSlotManager;
    [SerializeField] OverMenu overMenu;
    [SerializeField] ScreenUnitManager screenUnitManager;
    public static event Action UIInitiate;
    public static event Action UIUpdateAction;
    public static event Action GameIsWinAction;
    public static event Action GameIsLostAction;
    
    public float DealTimer { get; protected set; }
    public float GameTimer { get; protected set; }
    public bool GameRunning { get; protected set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //GameStart(1);
    }

    void Update()
    {
        if (!GameRunning)
        {
            return;
        }

        #region Card Generate Test

        //DealTimer -= Time.deltaTime;
        //if (DealTimer <= 0)
        //{
        //    DealTimer = 1;
        //    ResourcePackage c = GenerateNewCard(0);
        //    Debug.Log(string.Format("water:{0} sand:{1} foam:{2} action:{3}", c.Water, c.Sand, c.Foam, ((SpecialActionType)c.SpecialItem).ToString("G")));
        //}

        #endregion

        HandleTimer();

        HandleFire();

        UpdateUI();
    }

    #region Game Control

    public void GameStart(int level)
    {
        CurrentLevel = level;
        InitGameData(Levels[level - 1]);
        UpdateUI();
        OnboardCards = new ResourcePackage[Levels[level - 1].CardsOnBoard];
        for (int i = 0; i < OnboardCards.Length; i++)
        {
            GenerateNewCard(i);
        }
        if (UIInitiate != null)
        {
            UIInitiate();
        }
        GameRunning = true;
    }

    public void GamePause()
    {
        GameRunning = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    protected void GameOver(bool win)
    {
        GameRunning = false;
        Debug.Log(win ? "Won" : "Lost");

        if (win)
        {
            if (GameIsWinAction != null)
            {
                GameIsWinAction();
            }
        }
        else
        {
            if (GameIsLostAction != null)
            {
                GameIsLostAction();
            }
        }




    }

    #endregion

    #region Game Logic

    protected void InitGameData(Level level)
    {
        GameData = new GameData();
        
        GameData.MaxResources = level.MaxResources;
        GameData.AvailableResources = new ResourcePackage(level.InitialResources.Water, level.InitialResources.Sand, level.InitialResources.Foam);
        GameData.AvailableResources.SpecialAction = level.InitialResources.SpecialAction;

        GameData.DealInterval = level.DealInterval;

        GameData.InitialScore = level.InitialScore;
        GameData.RemianingScoreFloat = level.InitialScore;

        GameData.MaxFirePower = level.MaxFirePower;

        LoadFireInfo(level);
        ReadFireInfoAndAddFire();

        DealTimer = level.DealInterval;

        Debug.Log("Level " + CurrentLevel + " Initialized");
    }

    protected void LoadFireInfo(Level level)
    {
        foreach (FireInfo info in level.FireInfos)
        {
            GameData.WillHappenFires.Add(new FireInfo(info));
        }
    }

    protected void ReadFireInfoAndAddFire()
    {
        for (int i = 0; i < GameData.WillHappenFires.Count; i++)
        {
            FireInfo info = GameData.WillHappenFires[i];

            info.HappenTime -= Time.deltaTime;

            if (info.HappenTime <= 0)
            {
                GameData.OnboardFires.Add(FireFactory.GetFireOfType(info.Type, info.StartLevel,
                    info.EnlargeSpeed));

                Debug.LogWarning(info.Type.ToString("G") + " Type Fire Added");

                GameData.WillHappenFires.Remove(info);
                i--;
            }
        }
    }

    ///<summary>
    /// To judge if deal needed or game's time is up
    /// </summary> 
    protected void HandleTimer()
    {
        GameTimer += Time.deltaTime;

        DealTimer -= Time.deltaTime;
        if (DealTimer <= 0)
        {
            NewDealTriggered();
            DealTimer = GameData.DealInterval;
        }
    }

    /// <summary>
    /// New deal triggered by timer
    /// </summary>
    protected void NewDealTriggered()
    {
        GenerateNewCard(NextCardIndex);
        NextCardIndex += 1;
        NextCardIndex %= OnboardCards.Length;
    }

    /// <summary>
    /// Handle fire's development
    /// </summary>
    protected void HandleFire()
    {
        // Calculate current frame fire power
        GameData.RemianingScoreFloat -= GameData.TotalFirePower * Time.deltaTime;

        if (GameData.RemainingScore <= 0)
        {
            GameOver(false);
            Debug.Log("Everything burnt!");
            return;
        }

        // Increase current all fire's power
        GameData.IncreaseFirePower();

        if (GameData.TotalFirePower > GameData.MaxFirePower)
        {
            GameOver(false);
            Debug.Log("The world is too hot!");
            return;
        }

        // Add new fire into game if needed
        ReadFireInfoAndAddFire();

        foreach (Fire fire in GameData.OnboardFires)
        {
            if (!fire.Vanished)
            {
                // There's at least one fire still burning
                return;
            }
        }

        GameOver(true);
    }

    protected ResourcePackage GenerateNewCard(int index)
    {
        // Resource calculation needed
        // now just random

        bool isSpecialAction = Random.Range(0, 7) == 0;

        ResourcePackage card = null;

        if (isSpecialAction)
        {
            SpecialAction action = SpecialAction.GetRandomSpecialAction();
            card = new ResourcePackage(action.Type);
        }
        else
        {
            int waterCount = Random.Range(0, 6);
            int sandCount = Random.Range(0, 6 - waterCount);
            int foamCount = Random.Range((waterCount + sandCount == 0) ? 1 : 0, 
                6 - waterCount - sandCount);

            card = new ResourcePackage(waterCount, sandCount, foamCount);
        }

        OnboardCards[index] = card;
        SendCardToSlot(index, card);
        DealTimer = GameData.DealInterval;
        return card;
    }

    #endregion

    #region Communicate with UI

    protected void UpdateUI()
    {
        //FakeUI.Instance.UpdateUI();
        if (UIUpdateAction != null && GameRunning)
        {
            UIUpdateAction();
        }

    }

    protected void SendCardToSlot(int index, ResourcePackage card)
    {
        ResourcePackage cardRef = card;
        //FakeUI.Instance.UpdateCardInSlot(index, card);
        cardSlotManager.ChangeCard(index, card);

    }

    public bool CardSelected(int index)
    {
        Debug.Log("Received message from the UI Card num " + index);
        if (OnboardCards[index].SpecialAction != SpecialActionType.None)
        {
            Debug.Log("SA");
            // Trigger special action
            GameData.Instance.GetSpecialAction(OnboardCards[index].SpecialAction);

            // Deal
            DealTimer = GameData.DealInterval;
            GenerateNewCard(index);
            NextCardIndex = (NextCardIndex + 1) % OnboardCards.Length;

            return true;
        }
        else
        {
            bool validClick = false;

            int waterSum = 0;
            int sandSum = 0;
            int foamSum = 0;

            // Make sure which kinds of resources the card got
            bool staWater = OnboardCards[index].Water > 0;
            bool staSand = OnboardCards[index].Sand > 0;
            bool staFoam = OnboardCards[index].Foam > 0;

            // Calculate all resources amount
            for (int i = 0; i < OnboardCards.Length; i++)
            {
                if (staWater)
                {
                    waterSum += OnboardCards[i].Water;
                }

                if (staSand)
                {
                    sandSum += OnboardCards[i].Sand;
                }

                if (staFoam)
                {
                    foamSum += OnboardCards[i].Foam;
                }
            }

            // Judge if click is valid
            bool waterSatisfied = waterSum == GetItemNumber;
            bool sandSatisfied = sandSum == GetItemNumber;
            bool foamSatisfied = foamSum == GetItemNumber;

            if ( waterSatisfied || sandSatisfied || foamSatisfied)
            {
                GameData.AddResources(waterSatisfied, sandSatisfied, foamSatisfied);

                // Find which card should be replaced
                List<int> needNewCardIndex = new List<int>();
                for (int i = 0; i < OnboardCards.Length; i++)
                {
                    if (waterSatisfied && OnboardCards[i].Water > 0 && !needNewCardIndex.Contains(i))
                    {
                        needNewCardIndex.Add(i);
                    }

                    if (sandSatisfied && OnboardCards[i].Sand > 0 && !needNewCardIndex.Contains(i))
                    {
                        needNewCardIndex.Add(i);
                    }

                    if (foamSatisfied && OnboardCards[i].Foam > 0 && !needNewCardIndex.Contains(i))
                    {
                        needNewCardIndex.Add(i);
                    }
                }
                needNewCardIndex.Sort();
                foreach (int i in needNewCardIndex)
                {
                    GenerateNewCard(i);
                }

                NextCardIndex = (NextCardIndex + 1) % OnboardCards.Length;
                DealTimer = GameData.DealInterval;
                // GetItemNumber = Random.Range(GetItemNumberRange.x, GetItemNumberRange.y + 1);
                Debug.Log("NA");
                return true;
            }
            else
            {
                Debug.Log("Inv");
                return false;
            }
        }
    }

    public void ReDeal()
    {
        NewDealTriggered();
        GameData.AddHeat(Levels[CurrentLevel - 1].HeatAddByHand);
    }

    public SpecialActionType TriggerSpecialAction()
    {
        if (GameData.AvailableResources.SpecialAction != SpecialActionType.None)
        {
            SpecialActionType action = GameData.AvailableResources.SpecialAction;

            // Do Action
            GameData.TriggerSpecialAction();

            GameData.AvailableResources.SpecialAction = SpecialActionType.None;
            return action;
        }

        return SpecialActionType.None;
    }

    public void UseResources(ResourceType resourceType)
    {
        GameData.UseResource(resourceType);
    }

    #endregion
}
