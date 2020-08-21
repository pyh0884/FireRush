using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.LWRP;

public class ButtonAndSwitch : InteractiveObject
{
    public UnityEvent PushDown;
    public enum ButtonStatus { DEFAULT,HOVER,DOWN};
    protected ButtonStatus status;

    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite spr_default;
    [SerializeField] protected Sprite spr_down;

    [SerializeField] protected GameObject default_light;
    [SerializeField] protected GameObject down_light;

    protected AudioSystem audioSystem;
    [SerializeField] protected AudioClip[] clickAudio;
    [SerializeField] protected AudioClip[] hoverAudio;

    public override void OnMouseEnter()
    {
        status = ButtonStatus.HOVER;
        SetLightEffect(status);  //set effect to mouse hover
        PlayAudioEffect(status);
    }

    public override void OnMouseExit()  
    {
        spriteRenderer.sprite = spr_default;
        //change sprite to default

        status = ButtonStatus.DEFAULT;
        SetLightEffect(status);    //reset the effect to default
    }

    public override void OnMouseDown()
    {
        PushDown.Invoke();

        //change sprite to pushdown
        spriteRenderer.sprite = spr_down;

        status = ButtonStatus.DOWN;
        SetLightEffect(status);   //set effect to mouse down
        PlayAudioEffect(status);
    }

    public override void OnMouseUp()
    {
        spriteRenderer.sprite = spr_default;
        //change sprite to default

        status = ButtonStatus.HOVER;
        SetLightEffect(status);
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSystem = GetComponent<AudioSystem>();
    }

    protected void SetLightEffect(ButtonStatus status)
    {
        //set differ light and sound effect according to the button status
        switch (status)
        {
            case ButtonStatus.DEFAULT:
                //disable all light
                default_light.SetActive(false);
                down_light.SetActive(false);
                break;

            case ButtonStatus.HOVER:
                //default light
                default_light.SetActive(true);
                down_light.SetActive(false);
                break;

            case ButtonStatus.DOWN:
                //down light
                default_light.SetActive(false);
                down_light.SetActive(true);
                break;
        }
    }

    protected void PlayAudioEffect(ButtonStatus status)
    {
        switch (status)
        {
            case ButtonStatus.DEFAULT:
                break;

            case ButtonStatus.HOVER:
                audioSystem.PlayAudio(hoverAudio);
                break;

            case ButtonStatus.DOWN:
                audioSystem.PlayAudio(clickAudio);
                break;
        }
    }


}
