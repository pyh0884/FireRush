using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesSelector : MonoBehaviour
{
    public static ResourcesSelector Instance { get; protected set; }

    public ResourceType SelectedResource { get; protected set; }

    void Awake()
    {
        Instance = this;
    }

    public void ChangeType(ResourceType type)
    {
        SelectedResource = type;
    }
}
