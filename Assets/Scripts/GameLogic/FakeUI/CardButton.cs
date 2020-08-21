using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButton : MonoBehaviour
{
    public int Index;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Click()
    {
        if (!GameManager.Instance.CardSelected(Index))
        {
            Debug.LogWarning("Invalid Click");
        }
    }
}
