using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIcon : MonoBehaviour
{
    IEnumerator currentCorotine;

    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite spr_off;
    [SerializeField] Sprite spr_on;

    bool iconIsOn;

    IEnumerator IconFlick()
    {
        while(true)
        {
            if (iconIsOn)
            {
                spriteRenderer.sprite = spr_off;
                iconIsOn = false;
            }
            else
            {
                spriteRenderer.sprite = spr_on;
                iconIsOn = true;
            }
            //DebugExtension.DebugText("Change once");
            yield return new WaitForSeconds(0.8f);
        }   
    }

    public void SetStatus(bool status)
    {
        if (status) 
        {
            currentCorotine = IconFlick();
            StartCoroutine(currentCorotine);
        }
        else
        {
            if (currentCorotine != null)
            {
                StopCoroutine(currentCorotine);
                currentCorotine = null;

            }
            
            spriteRenderer.sprite = spr_off;
        }

    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();        
    }

}
