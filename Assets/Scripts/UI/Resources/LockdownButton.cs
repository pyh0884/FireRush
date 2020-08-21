using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockdownButton : ButtonAndSwitch
{

    public override void OnMouseUp()
    {
        //base.OnMouseUp();
        //do nothing, only reset on function called
    }

    public override void OnMouseEnter()
    {
        //set effect only
        if (status == ButtonStatus.DOWN)
        {
            return;
        }
        else
        {
            status = ButtonStatus.HOVER;
            SetLightEffect(status);
            PlayAudioEffect(status);
        }
    }

    public override void OnMouseExit()
    {
        //set effect only
        if (status == ButtonStatus.DOWN)
        {
            return;
        }
        else
        {
            status = ButtonStatus.DEFAULT;
            SetLightEffect(status);
        }

    }



    public void ResetStatus()
    {
        spriteRenderer.sprite = spr_default;
        status = ButtonStatus.DEFAULT;
        SetLightEffect(status);
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSystem = GetComponent<AudioSystem>();

        //use a action to reset the lockdown button
    }

}
