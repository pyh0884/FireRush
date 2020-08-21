using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public bool IsSpecial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Click()
    {
        if (IsSpecial)
        {
            Debug.Log(GameManager.Instance.TriggerSpecialAction().ToString("G"));
        }
        else
        {
            GameManager.Instance.UseResources(ResourcesSelector.Instance.SelectedResource);
        }
    }
}
