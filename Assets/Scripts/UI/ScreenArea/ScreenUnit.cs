using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUnit : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite spr_default;
    [SerializeField] Sprite spr_default_after;
    [SerializeField] Sprite[] spr_fire = new Sprite[2];
    [SerializeField] Sprite[] spr_electric = new Sprite[2];
    [SerializeField] Sprite[] spr_gas = new Sprite[2];
    [SerializeField] Sprite[] spr_oil = new Sprite[2];
    [SerializeField] Sprite spr_ash = null;

    public enum UnitStatus { DEFAULT,FIRE,ELECTRIC,GAS,OIL,ASH };
    public UnitStatus currentStatus;
    public int currentFireSize = 0;
    bool isInitiate;

    public const int SMALL_FIRE = 0;
    public const int BIG_FIRE = 1;

    Sprite statusSprite;

    bool needFlick = false;
    bool isFlick = false;

    public Vector2Int posIndex;

    public void IconFlick()
    {
        if (needFlick)
        {
            if (isFlick)
            {
                spriteRenderer.sprite = spr_default;
                isFlick = false;
            }
            else
            {
                spriteRenderer.sprite = statusSprite;
                isFlick = true;
            }
        }
    }
    
    //true to start flicking, false to stop the flick
    void SetFlick(bool value)
    {
        if (value)  
        {
            needFlick = true;
        }
        else
        {

            needFlick = false;
            isFlick = false;
        }
    }

    public void SetUnitStats(UnitStatus status, int fireindex)
    {
        currentStatus = status;
        currentFireSize = fireindex;

        if (isInitiate == false)
        {
            return; //save the status, wait for the next self calling refresh in Start function
        }

        switch (status)
        {
            case UnitStatus.DEFAULT:
                SetFlick(false);
                statusSprite = spr_default;
                spriteRenderer.sprite = statusSprite;
                break;

            case UnitStatus.FIRE:
                if (fireindex == SMALL_FIRE)
                {
                    statusSprite = spr_fire[SMALL_FIRE];
                    spr_default = spr_default_after;
                    SetFlick(true);
                }
                else
                {
                    statusSprite = spr_fire[BIG_FIRE];
                    spr_default = spr_default_after;
                    SetFlick(true);
                }
                break;

            case UnitStatus.ELECTRIC:
                if (fireindex == SMALL_FIRE)
                {
                    statusSprite = spr_electric[SMALL_FIRE];
                    spr_default = spr_default_after;
                    SetFlick(true);
                }
                else
                {
                    statusSprite = spr_electric[BIG_FIRE];
                    spr_default = spr_default_after;
                    SetFlick(true);
                }
                break;


            case UnitStatus.GAS:
                if (fireindex == SMALL_FIRE)
                {
                    statusSprite = spr_gas[SMALL_FIRE];
                    spr_default = spr_default_after;
                    SetFlick(true);
                }
                else
                {
                    statusSprite = spr_gas[BIG_FIRE];
                    spr_default = spr_default_after;
                    SetFlick(true);
                }
                break;


            case UnitStatus.OIL:
                if (fireindex == SMALL_FIRE)
                {
                    statusSprite = spr_oil[SMALL_FIRE];
                    spr_default = spr_default_after;
                    SetFlick(true);
                }
                else
                {
                    statusSprite = spr_oil[BIG_FIRE];
                    spr_default = spr_default_after;
                    SetFlick(true);
                }
                break;

            case UnitStatus.ASH:
                statusSprite = spr_ash;
                spriteRenderer.sprite = statusSprite;
                SetFlick(false);
                break;
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        statusSprite = spr_default;

        ScreenUnitManager.ScreenUnitFlick += IconFlick;

        //finish initiate
        isInitiate = true;
        SetUnitStats(currentStatus, currentFireSize);

    }

    private void OnDestroy()
    {
        ScreenUnitManager.ScreenUnitFlick -= IconFlick;
    }

    #region DebugSector
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetUnitStats((UnitStatus)Random.Range(0, 10),Random.Range(0,1 + 1));
        }
    }
    */
    #endregion
}
