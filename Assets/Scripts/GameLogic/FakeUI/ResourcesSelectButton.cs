using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesSelectButton : MonoBehaviour
{
    public ResourceType Type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Click()
    {
        ResourcesSelector.Instance.ChangeType(Type);
    }
}
