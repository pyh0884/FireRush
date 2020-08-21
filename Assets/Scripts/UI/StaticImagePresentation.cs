using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticImagePresentation : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] staticImage;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetImage(Sprite spr_newImage)
    {
        spriteRenderer.sprite = spr_newImage;
    }

    public void SetImage(int imageIndex)
    {
        spriteRenderer.sprite = staticImage[imageIndex];
    }
}
