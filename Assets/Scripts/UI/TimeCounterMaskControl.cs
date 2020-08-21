using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounterMaskControl : MonoBehaviour
{
    [SerializeField] Transform fullPosition;
    [SerializeField] Transform zeroPosition;
    [SerializeField] Transform t_mask;

    [SerializeField] float maxValue;
    [SerializeField] float minValue;

    public void CounterInitiate(float min,float max)
    {
        minValue = min;
        maxValue = max;
    }

    public void SetMaskPosition(float currentValue)
    {
        float posFix = currentValue / maxValue;

        t_mask.position = zeroPosition.position + posFix * (fullPosition.position - zeroPosition.position);
    }



    #region DebugSector
    /*
    [SerializeField] [Range(0, 100)] float debugValue;

    private void Update()
    {
        SetMaskPosition(debugValue);
    }
    */
    #endregion


}
