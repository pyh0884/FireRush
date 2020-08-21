using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBarControl : MonoBehaviour
{
    [SerializeField] int maxValue;
    [SerializeField] int currentValue;

    Vector3 posFix = new Vector3(0f,5f,0f);   //delta of postion between each bar

    float delay;

    //set these parameter in the inspector
    [SerializeField] GameObject barPrefab;  
    [SerializeField] Sprite spr_empty;
    [SerializeField] Sprite spr_full;

    GameObject spawnBar;
    ResourceBar spawnResourceBar;

    List<ResourceBar> bars = new List<ResourceBar>();

    // Start is called before the first frame update
    void Start()
    {
        //BarInitialize();
    }

    public void BarInitialize(int maxValueSetting)
    {
        maxValue = maxValueSetting;
        currentValue = 0;
        Vector3 spawnPosition = transform.position; //get the first bar location

        for (int i = 0; i < maxValue; i++)
        {
            //spawn a bar
            spawnBar = Instantiate(barPrefab, spawnPosition, this.transform.rotation, this.transform);
            spawnBar.layer = InterfaceParameters.SCREEN_LAYER_NUM;
            spawnResourceBar = spawnBar.GetComponent<ResourceBar>();
            //add the bar to the list
            bars.Add(spawnResourceBar);
            //set the spr of each bar status
            bars[i].Initiate(spr_empty, spr_full);

            //recalculate the next spawn position
            spawnPosition += posFix;
        }
    }

    public void ResetBar(int newMaxValue)
    {
        for (int i = 0; i < bars.Count; i++)
        {
            Destroy(bars[i].gameObject);
        }
        bars.Clear();

        maxValue = newMaxValue;
        currentValue = 0;

        BarInitialize(newMaxValue);
    }

    public void ResetBar()
    {
        for (int i = 0; i < bars.Count; i++)
        {
            Destroy(bars[i].gameObject);
        }
        bars.Clear();
    }


    public void RefreshBar(int value)
    {
        //remeber the index is start from 0
        //DebugExtension.DebugText("Receive Value {0}", value);
        if (value == currentValue)
        {
            //do nothing, no need to refresh
            return;
        }
        else if (value > currentValue)
        {
            //update the bar which index is higher than the currentvalue(include) and smaller(exclude) than the value
            for (int i = currentValue; i < value; i++)//refresh the bar from lower to higher
            {
                bars[i].SetSprite(ResourceBar.Status.FULL);
            }
            currentValue = value;
        }
        else //value < currentValue
        {
            //update the bar which index is higher than the value(include) and smaller(exclude) than the currentvalue
            for (int i = currentValue - 1; i >= value; i--)
            {
                bars[i].SetSprite(ResourceBar.Status.EMPTY);
            }
            currentValue = value;
        }
    }

    #region DebugSector
    /*
    [SerializeField] int debugValue;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RefreshBar(debugValue);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBar(maxValue);
        }
    }
    */
    #endregion
}
