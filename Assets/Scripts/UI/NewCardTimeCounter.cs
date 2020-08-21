using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCardTimeCounter : MonoBehaviour
{
    TimeCounterMaskControl controler;
    public void UIInitiate()
    {
        //set min and max value of the counter
        controler.CounterInitiate(0f, (int)GameData.Instance.DealInterval);
    }

    public void RefreshTimeCounter()
    {
        controler.SetMaskPosition(GameManager.Instance.DealTimer);
    }

    void Start()
    {
        controler = GetComponent<TimeCounterMaskControl>();


        GameManager.UIInitiate += UIInitiate;
        GameManager.UIUpdateAction += RefreshTimeCounter;

    }

    private void OnDestroy()
    {
        GameManager.UIInitiate -= UIInitiate;
        GameManager.UIUpdateAction -= RefreshTimeCounter;
    }


}
