using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBar : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    Sprite spr_empty;
    Sprite spr_full;

    public enum Status { EMPTY,FULL};

    public void Initiate(Sprite empty, Sprite full)
    {
        spr_empty = empty;
        spr_full = full;
    }

    public void SetSprite(ResourceBar.Status barStatus)
    {
        switch (barStatus)
        {
            case Status.EMPTY:
                spriteRenderer.sprite = spr_empty;
                break;
            case Status.FULL:
                spriteRenderer.sprite = spr_full;
                break;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

}
