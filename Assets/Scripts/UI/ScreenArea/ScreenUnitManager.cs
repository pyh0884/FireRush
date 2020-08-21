using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenUnitManager : MonoBehaviour
{
    Dictionary<Vector2Int, ScreenUnit> dict_screenUnit = new Dictionary<Vector2Int, ScreenUnit>();  //include all the screen unit for search
    Vector2Int maxPosIndex = new Vector2Int(14,9);
    public static event Action ScreenUnitFlick;
    bool isInitiate;
    IEnumerator flickCorotine;


    List<ScreenUnit> defaultUnits = new List<ScreenUnit>();
    List<ScreenUnit> ashUnits = new List<ScreenUnit>();
    List<fireStatus> allFireStatus = new List<fireStatus>();

    [SerializeField] GameObject screenUnitPrefab;


    class fireStatus
    {
        public int fireUnitCount;// Mathf.floor(fire level * enlarge factor)

        public FireType firetype;

        public List<ScreenUnit> fireUnits = new List<ScreenUnit>();  //usage currentFires[FireList Index][Unit Index]
    }

    public int totalFireUnits()
    {
        int count = 0;
        for (int i = 0; i < allFireStatus.Count; i++)
        {
            count += allFireStatus[i].fireUnits.Count;
        }
        return count;
    }

    public int[] RandomIndex(int length)
    {
        int[] indexs = new int[length];
        for (int i = 0; i < length; i++) //generate a normal order index sequence
        {
            indexs[i] = i;
        }

        for (int i = 0; i < length; i++)    //each loop generate a random index to switch
        {
            int temp = indexs[i];
            int randomIndex = UnityEngine.Random.Range(0, length);
            indexs[i] = indexs[randomIndex];
            indexs[randomIndex] = temp;
        }
        /*DebugExtension.DebugText("The random icon index are below");
        for (int i = 0; i < length; i++)
        {
            DebugExtension.DebugText("the NO.{0} index is: {1}", i, indexs[i]);
        }*/
        return indexs;
    }


    public ScreenUnit FindRelatedUnit(ScreenUnit.UnitStatus targetStatus, Vector2Int originalPosIndex)
    {
        Vector2Int[] posfix = new Vector2Int[4]{new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1)};
        int[] randomPosfixIndex = RandomIndex(4);

        for (int i = 0; i < randomPosfixIndex.Length; i++)
        {
            if (CheckUnitStatus(targetStatus, originalPosIndex + posfix[randomPosfixIndex[i]]))
            {
                //DebugExtension.DebugText("Find a related {0} unit from {1}", targetStatus, originalPosIndex);
                return dict_screenUnit[originalPosIndex + posfix[randomPosfixIndex[i]]];
            }
        }

        //DebugExtension.DebugText("{0} don't have {1} status Related Unit, try to get a random one", targetStatus, originalPosIndex);

        ScreenUnit randomTarget;
        switch (targetStatus)
        {
            case ScreenUnit.UnitStatus.DEFAULT:
                if (defaultUnits.Count != 0)
                {
                    randomTarget = defaultUnits[UnityEngine.Random.Range(0,defaultUnits.Count)];
                    return randomTarget;
                }
                break;
            case ScreenUnit.UnitStatus.ASH:
                if (ashUnits.Count != 0)
                {
                    randomTarget = ashUnits[UnityEngine.Random.Range(0, ashUnits.Count)];
                    return randomTarget;
                }
                break;
            case ScreenUnit.UnitStatus.FIRE:
                return FindRandomFireUnit(FireType.Normal);
            case ScreenUnit.UnitStatus.ELECTRIC:
                return FindRandomFireUnit(FireType.Electric);
            case ScreenUnit.UnitStatus.GAS:
                return FindRandomFireUnit(FireType.Gas);
            case ScreenUnit.UnitStatus.OIL:
                return FindRandomFireUnit(FireType.Oil);
            default:
                break;
        }

        //DebugExtension.DebugText("Failed to find the {0}, from {1}", targetStatus, originalPosIndex);
        return null;
    }

    public bool CheckUnitStatus(ScreenUnit.UnitStatus targetStatus, Vector2Int targetPosIndex)
    {
        if (dict_screenUnit.TryGetValue(targetPosIndex, out ScreenUnit target))
        {
            //target posIndex is not null
            if (target.currentStatus == targetStatus) //check status
            {
                //DebugExtension.DebugText("Find a {0} unit at {1}", targetStatus, targetPosIndex);
                return true;    //status correct
            }
            else
            {
                return false;   //status wrong
            }
        }

        //fail to find the PosIndex
        return false;
    }

    public ScreenUnit FindRandomFireUnit() //all random
    {
        int randomFireStatusIndex = UnityEngine.Random.Range(0, allFireStatus.Count);
        int randomFireUnitIndex = UnityEngine.Random.Range(0, allFireStatus[randomFireStatusIndex].fireUnits.Count);
        return allFireStatus[randomFireStatusIndex].fireUnits[randomFireUnitIndex];
    }

    public ScreenUnit FindRandomFireUnit(FireType targetFireType)   //with type condition
    {
        foreach (fireStatus fireStatus in allFireStatus)//check each fire list type and unit
        {
            if (fireStatus.firetype == targetFireType)
            {
                //DebugExtension.DebugText("Find a Random {0} unit at {1}", targetFireType, fireStatus.fireUnits[UnityEngine.Random.Range(0, fireStatus.fireUnits.Count)].posIndex);
                return fireStatus.fireUnits[UnityEngine.Random.Range(0, fireStatus.fireUnits.Count)];
            }
        }
        return null;    //fail to find target fire type screen unit
    }

    public void SpawnScreenUnits()
    {
        isInitiate = false;
        //clean before initiate
        CleanScreenUnits();

        GameObject obj_spawnScreenUnit;
        Vector3 spawnPosition;
        //spawn all the screen units, save them in dictionary
        for (int y = 0; y < maxPosIndex.y; y++)
        {
            for (int x = 0; x < maxPosIndex.x; x++)
            {
                spawnPosition = transform.position + new Vector3(x * 10, y * 10, 0);
                obj_spawnScreenUnit = GameObject.Instantiate(screenUnitPrefab,spawnPosition, transform.rotation, transform);
                dict_screenUnit.Add(new Vector2Int(x, y),obj_spawnScreenUnit.GetComponent<ScreenUnit>());
                dict_screenUnit[new Vector2Int(x, y)].posIndex = new Vector2Int(x, y);
                defaultUnits.Add(dict_screenUnit[new Vector2Int(x, y)]);
            }
        }

        //spawn finished, start flicking
        isInitiate = true;
        StartCoroutine(flickCorotine);
    }

    public void CleanScreenUnits()
    {
        isInitiate = false;
        StopCoroutine(flickCorotine);

        defaultUnits.Clear();
        ashUnits.Clear();
        allFireStatus.Clear();

        for (int y = 0; y < maxPosIndex.y; y++)
        {
            for (int x = 0; x < maxPosIndex.x; x++)
            {
                if (dict_screenUnit.TryGetValue(new Vector2Int(x, y),out ScreenUnit unit))
                {
                    Destroy(dict_screenUnit[new Vector2Int(x, y)].gameObject);
                }
            }
        }
        dict_screenUnit.Clear();
    }

    public void RefreshScreenUnits()
    {
        //check if all the screen units is spawned
        if (isInitiate == false)
        {
            return; //stop currend frame refresh function
        }

        //Check if everything is burnt out
        if (GameData.Instance.RemainingScore == 0)
        {
            CleanScreenUnits();
        }

        //Check if all the fire has been put out
        bool isPutout = true;
        foreach (Fire fire in GameData.Instance.OnboardFires)
        {
            if (fire.CurrentFireLevel > 0)
            {
                isPutout = false;
                break;
            }
        }

        if (isPutout)
        {
            if (allFireStatus.Count == 0)
            {
                //no fire present, stop refresh immediately
                return;
            }
            
            //set all current on board fire to normal and stop refresh
            for (int fireStatusIndex = 0; fireStatusIndex < allFireStatus.Count; fireStatusIndex++)
            {
                for (int fireUnitIndex = 0; fireUnitIndex < allFireStatus[fireStatusIndex].fireUnits.Count; fireUnitIndex++)
                {
                    allFireStatus[fireStatusIndex].fireUnits[fireUnitIndex].SetUnitStats(ScreenUnit.UnitStatus.DEFAULT, 0);

                    //add this fire unit into default list
                    defaultUnits.Add(allFireStatus[fireStatusIndex].fireUnits[fireUnitIndex]);
                }
                //update fire type
                allFireStatus[fireStatusIndex].firetype = GameData.Instance.OnboardFires[fireStatusIndex].Type;

                //reset the unitcounter
                allFireStatus[fireStatusIndex].fireUnitCount = 0;

                //clean the current loop fire list
                allFireStatus[fireStatusIndex].fireUnits.Clear();
            }
            return; //stop refresh function
        }

        //refresh current On board fire
        for (int fireStatusIndex = 0; fireStatusIndex < allFireStatus.Count; fireStatusIndex++)
        {
            int deltaUnitAmount = Mathf.FloorToInt(GameData.Instance.OnboardFires[fireStatusIndex].FireLevelInt * GameData.Instance.OnboardFires[fireStatusIndex].EnlargeFactor * 5) - allFireStatus[fireStatusIndex].fireUnitCount;
            if (deltaUnitAmount == 0)
            {
                if (allFireStatus[fireStatusIndex].firetype != GameData.Instance.OnboardFires[fireStatusIndex].Type) //check fire type
                {
                    allFireStatus[fireStatusIndex].firetype = GameData.Instance.OnboardFires[fireStatusIndex].Type; //update fire type

                    //set all the screenunit fire status of current fireunits
                    for (int fireUnitIndex = 0; fireUnitIndex < allFireStatus[fireStatusIndex].fireUnits.Count; fireUnitIndex++)
                    {
                        if (fireUnitIndex % 2 == allFireStatus[fireStatusIndex].fireUnits.Count % 2)
                        {
                            allFireStatus[fireStatusIndex].fireUnits[fireUnitIndex].SetUnitStats(FireTypeToScreenUnitStatus(allFireStatus[fireStatusIndex].firetype), ScreenUnit.SMALL_FIRE);
                        }
                        else
                        {
                            allFireStatus[fireStatusIndex].fireUnits[fireUnitIndex].SetUnitStats(FireTypeToScreenUnitStatus(allFireStatus[fireStatusIndex].firetype), ScreenUnit.BIG_FIRE);
                        }
                    }
                }

            }
            else if (deltaUnitAmount > 0)
            {
                //need spawn more fires
                for (allFireStatus[fireStatusIndex].fireUnitCount += deltaUnitAmount; allFireStatus[fireStatusIndex].fireUnits.Count < allFireStatus[fireStatusIndex].fireUnitCount;)
                {
                    if (defaultUnits.Count != 0)    //find a related default unit
                    {
                        int randomOriginalFirePosIndex = UnityEngine.Random.Range(0, allFireStatus[fireStatusIndex].fireUnits.Count); //find a random fire index in current list as the source of enlarge fire

                        //get a related default unit
                        ScreenUnit relatedDefaultUnit = FindRelatedUnit(ScreenUnit.UnitStatus.DEFAULT, allFireStatus[fireStatusIndex].fireUnits[randomOriginalFirePosIndex].posIndex);

                        //move the unit from defaultUnit list to the fireUnit list
                        allFireStatus[fireStatusIndex].fireUnits.Add(relatedDefaultUnit);
                        defaultUnits.Remove(relatedDefaultUnit);
                    }
                    else
                    {
                        break; //stop fire spawn
                    }
                }

                //set all the screenunit fire status of current fireunits
                allFireStatus[fireStatusIndex].firetype = GameData.Instance.OnboardFires[fireStatusIndex].Type; //update fire type
                for (int fireUnitIndex = 0; fireUnitIndex < allFireStatus[fireStatusIndex].fireUnits.Count; fireUnitIndex++)
                {
                    if (fireUnitIndex % 2 == allFireStatus[fireStatusIndex].fireUnits.Count % 2)
                    {
                        allFireStatus[fireStatusIndex].fireUnits[fireUnitIndex].SetUnitStats(FireTypeToScreenUnitStatus(allFireStatus[fireStatusIndex].firetype), ScreenUnit.SMALL_FIRE);
                    }
                    else
                    {
                        allFireStatus[fireStatusIndex].fireUnits[fireUnitIndex].SetUnitStats(FireTypeToScreenUnitStatus(allFireStatus[fireStatusIndex].firetype), ScreenUnit.BIG_FIRE);
                    }
                }
            }
            else
            {
                //need set fire to default
                for (allFireStatus[fireStatusIndex].fireUnitCount += deltaUnitAmount; allFireStatus[fireStatusIndex].fireUnits.Count > allFireStatus[fireStatusIndex].fireUnitCount;)
                {
                    //find a random fire in current fire list
                    int randomFireUnitIndex = UnityEngine.Random.Range(0, allFireStatus[fireStatusIndex].fireUnits.Count); 
                    ScreenUnit randomFireUnit = allFireStatus[fireStatusIndex].fireUnits[randomFireUnitIndex];

                    //set this fire to default status
                    randomFireUnit.SetUnitStats(ScreenUnit.UnitStatus.DEFAULT, 0);

                    //add this fire unit into default list
                    defaultUnits.Add(randomFireUnit);
                    //remove this unit from current fire list
                    allFireStatus[fireStatusIndex].fireUnits.RemoveAt(randomFireUnitIndex);
                }

                //set all the screenunit fire status to current fireunits
                allFireStatus[fireStatusIndex].firetype = GameData.Instance.OnboardFires[fireStatusIndex].Type; //update fire type
                for (int fireUnitIndex = 0; fireUnitIndex < allFireStatus[fireStatusIndex].fireUnits.Count; fireUnitIndex++)
                {
                    if (fireUnitIndex % 2 == allFireStatus[fireStatusIndex].fireUnits.Count % 2)
                    {
                        allFireStatus[fireStatusIndex].fireUnits[fireUnitIndex].SetUnitStats(FireTypeToScreenUnitStatus(allFireStatus[fireStatusIndex].firetype), ScreenUnit.SMALL_FIRE);
                    }
                    else
                    {
                        allFireStatus[fireStatusIndex].fireUnits[fireUnitIndex].SetUnitStats(FireTypeToScreenUnitStatus(allFireStatus[fireStatusIndex].firetype), ScreenUnit.BIG_FIRE);
                    }
                }

            }
        }

        //detect fire number, check if need add new fire
        if (allFireStatus.Count < GameData.Instance.OnboardFires.Count)
        {

            //need spawn new fire (one or more)
            for (int fireStatusIndex = allFireStatus.Count; fireStatusIndex < GameData.Instance.OnboardFires.Count; fireStatusIndex++)
            {
                //add new fire to the list
                allFireStatus.Add(new fireStatus());

                //set the parameter of the new fire
                allFireStatus[fireStatusIndex].firetype = GameData.Instance.OnboardFires[fireStatusIndex].Type;

                //spawn fire units
                for (int targetUnitAmout = Mathf.FloorToInt(GameData.Instance.OnboardFires[fireStatusIndex].FireLevelInt * GameData.Instance.OnboardFires[fireStatusIndex].EnlargeFactor); allFireStatus[fireStatusIndex].fireUnitCount < targetUnitAmout; allFireStatus[fireStatusIndex].fireUnitCount++)
                {
                    if (defaultUnits.Count != 0)    //find a random default unit
                    {
                        //get a random index 
                        int randomIndex = UnityEngine.Random.Range(0, defaultUnits.Count);

                        //move the unit from defaultUnit list to the fireUnit list
                        allFireStatus[fireStatusIndex].fireUnits.Add(defaultUnits[randomIndex]);
                        defaultUnits.RemoveAt(randomIndex);
                    }
                }

                //set each unit in fireunit list
                for (int fireUnitIndex = 0; fireUnitIndex < allFireStatus[fireStatusIndex].fireUnits.Count; fireUnitIndex++)
                {
                    if (fireUnitIndex % 2 == allFireStatus[fireStatusIndex].fireUnits.Count % 2)
                    {
                        allFireStatus[fireStatusIndex].fireUnits[fireUnitIndex].SetUnitStats(FireTypeToScreenUnitStatus(allFireStatus[fireStatusIndex].firetype), ScreenUnit.SMALL_FIRE);
                    }
                    else
                    {
                        allFireStatus[fireStatusIndex].fireUnits[fireUnitIndex].SetUnitStats(FireTypeToScreenUnitStatus(allFireStatus[fireStatusIndex].firetype), ScreenUnit.BIG_FIRE);
                    }
                }
            }
        }

        //Ash Spawn
        int targetDefaultUnit = Mathf.FloorToInt(maxPosIndex.x * maxPosIndex.y * GameData.Instance.RemianingScoreFloat / GameData.Instance.InitialScore);
        int spawnAshUnitCount = defaultUnits.Count + totalFireUnits() - targetDefaultUnit;
        //DebugExtension.DebugText("spawnAshUnit is {0},targetDefaultUnit is {1}", spawnAshUnitCount, targetDefaultUnit);


        for (int spawnCount = 0; spawnCount < spawnAshUnitCount; spawnCount++)
        {
            if (defaultUnits.Count == 0 || allFireStatus.Count == 0)
            {
                break;
            }

            int randomFireStatusIndex = UnityEngine.Random.Range(0, allFireStatus.Count);
            int randomFireUnitIndex = UnityEngine.Random.Range(0, allFireStatus[randomFireStatusIndex].fireUnits.Count);
            //DebugExtension.DebugText("randomFireStatusIndex");
            ScreenUnit randomFireUnit = allFireStatus[randomFireStatusIndex].fireUnits[randomFireUnitIndex];
            ScreenUnit newDefaultUnit = FindRelatedUnit(ScreenUnit.UnitStatus.DEFAULT, randomFireUnit.posIndex);

            //set the newDefaultUnit into a fire unit same as the current fire unit
            newDefaultUnit.SetUnitStats(randomFireUnit.currentStatus, randomFireUnit.currentFireSize);
            //set the randomFireUnit into ashUnit
            randomFireUnit.SetUnitStats(ScreenUnit.UnitStatus.ASH, 0);


            //randomfireUnit  remove from current fire list, add into the ash list
            allFireStatus[randomFireStatusIndex].fireUnits.Remove(randomFireUnit);
            ashUnits.Add(randomFireUnit);
            //new DefaultUnit remove from defaultunit list, add into current fire list
            defaultUnits.Remove(newDefaultUnit);
            allFireStatus[randomFireStatusIndex].fireUnits.Add(newDefaultUnit);

        }
    }

    IEnumerator SendFlickMessage()
    {
        while (true)
        {
            if (ScreenUnitFlick != null)
            {
                ScreenUnitFlick();
            }
            yield return new WaitForSeconds(0.5f);
        }
        
    }


    public ScreenUnit.UnitStatus FireTypeToScreenUnitStatus(FireType fireType)
    {
        switch (fireType)
        {
            case FireType.Normal:
                return ScreenUnit.UnitStatus.FIRE;
            case FireType.Electric:
                return ScreenUnit.UnitStatus.ELECTRIC;
            case FireType.Gas:
                return ScreenUnit.UnitStatus.GAS;
            case FireType.Oil:
                return ScreenUnit.UnitStatus.OIL;
        }
        return ScreenUnit.UnitStatus.FIRE;
    }

    private void Start()
    {
        flickCorotine = SendFlickMessage();

        GameManager.UIInitiate += SpawnScreenUnits;
        GameManager.UIUpdateAction += RefreshScreenUnits;

        GameManager.GameIsLostAction += RefreshScreenUnits;
        GameManager.GameIsWinAction += RefreshScreenUnits;


    }

    private void OnDestroy()
    {
        GameManager.UIInitiate -= SpawnScreenUnits;
        GameManager.UIUpdateAction -= RefreshScreenUnits;

        GameManager.GameIsLostAction -= RefreshScreenUnits;
        GameManager.GameIsWinAction -= RefreshScreenUnits;
    }

    #region DebugSector
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RefreshScreenUnits();
            DebugExtension.DebugText("Score left {0}", GameData.Instance.RemainingScore);
        }
    }*/
    #endregion

}
