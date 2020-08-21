using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTimeCounter : MonoBehaviour
{
    TimeCounterMaskControl controler;
    void Start()
    {
        controler = GetComponent<TimeCounterMaskControl>();

        GameManager.UIInitiate += UIInitiate;
        GameManager.UIUpdateAction += RefreshTimeCounter;

    }

    public void UIInitiate()
    {
        //set min and max value of the counter
        controler.CounterInitiate(0f, (int)GameData.Instance.MaxFirePower);
    }

    public void RefreshTimeCounter()
    {
        controler.SetMaskPosition(GameData.Instance.TotalFirePower);
    }
}
