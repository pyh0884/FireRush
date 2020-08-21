using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] bool isInteractable;
    [SerializeField] bool isPassThrough;
    [HideInInspector]public string longpressTag = null;

    public bool GetInteractable() {
        return isInteractable;
    }

    public void SetInteractable(bool setvalue) {
        isInteractable = setvalue;
    }

    public bool GetPassThrough()
    {
        return isPassThrough;
    }

    public void SetPassThrough(bool setvalue)
    {
        isPassThrough = setvalue;
    }

    /********************Basic Finger Gesture Function********************/

    public virtual void Tap(float tapPosX,float tapPosY)
    {
        DebugExtension.DebugText("{0} is being Tapped, tapPos is {1},{2}", this.gameObject.name,tapPosX,tapPosY);

        if (!isInteractable)
        {
            return;
        }
    }

    public virtual void DoubleTap(float tapPosX, float tapPosY)
    {
        DebugExtension.DebugText("{0} is being Double Tapped, doubletapPos is {1},{2}", this.gameObject.name, tapPosX, tapPosY);

        if (!isInteractable)
        {
            return;
        }
    }

    public virtual void Swipe(float startX, float startY, float endX, float endY, float velocityX, float velocityY)
    {
        DebugExtension.DebugText("{0} is being Swiped", this.gameObject.name);

        if (!isInteractable)
        {
            return;
        }
    }

    public virtual void OnMouseEnter()
    {
        if (!isInteractable)
        {
            return;
        }
        DebugExtension.DebugText("Mouse enter the {0}", this.gameObject.name);
    }

    public virtual void OnMouseExit()
    {
        if (!isInteractable)
        {
            return;
        }
        DebugExtension.DebugText("Mouse exit the {0}", this.gameObject.name);
    }

    public virtual void OnMouseDown()
    {
        DebugExtension.DebugText("Mouse Down at {0}", this.gameObject.name);
        if (!isInteractable)
        {
            return;
        }
    }

    public virtual void OnMouseUp()
    {
        DebugExtension.DebugText("Mouse Up at {0}", this.gameObject.name);
        if (!isInteractable)
        {
            return;
        }
    }





}
