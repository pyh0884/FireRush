using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Add this to each card model
/// This class describe the card resouce and card settings
/// </summary>
public class CardBehavior : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] spr_Icons;

    float _timeStamp;    //storage the time of spawn
    public float timeStamp
    {
        get
        {
            return _timeStamp;
        }
        set
        {
            _timeStamp = value;
        }
    }

    public void SetIcons(int iconIndex,int iconType)    //change the designated icon
    {
        if (iconIndex >= spr_Icons.Length)
        {
            DebugExtension.DebugText("Wrong iconIndex, value is {0}, position slot num {1}",iconIndex, spr_Icons.Length);  //prevent out of range memory access

            //leave default big red square icons on the card
            return;
        }

        spr_Icons[iconIndex].sprite = CardAttribute._instance.itemIcon[iconType];
    }

    public void SetSpecialIcons(int iconIndex,int specialIconIndex)
    {
        //DebugExtension.DebugText("SetSpecialIcons Function, index is {0}",specialIconIndex);
        //spr_Icons[0].sprite = CardAttribute._instance.specialItemIcon[specialIconIndex];    //change the only one icon slot to special item icon
        spr_Icons[iconIndex].sprite = CardAttribute._instance.specialActionIcon[specialIconIndex];
    }


}
