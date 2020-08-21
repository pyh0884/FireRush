using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelection : MonoBehaviour
{
    [SerializeField] ItemIcon[] itemIcons;
    bool[] itemStatus = new bool[3];    //index start from 0, 2 items aquire recording

    public void UseSpecialAction(int index)
    {
        if (itemStatus[index] == false)
        {
            DebugExtension.DebugText("You don't have {0}", (SpecialActionType)index);
        }
        else
        {
            DebugExtension.DebugText("Use a Special action {0}", (SpecialActionType)index);
            //send message to other funcion

            GameManager.Instance.TriggerSpecialAction();
        }

    }

    public void SetItemStatus(int index,bool status)
    {
        if (itemStatus[index] == status)
        {
            return;
        }
        else
        {
            itemStatus[index] = status;
            itemIcons[index].SetStatus(status);
        }
    }

    public void RefreshItemStatus()
    {
        SpecialActionType specialActionType = GameData.Instance.AvailableResources.SpecialAction; //get the item type from the gamedata

        //switch the item type and set the button
        switch (specialActionType)
        {
            case SpecialActionType.None:
                SetItemStatus((int)SpecialActionType.CutOffCircuit, false);
                SetItemStatus((int)SpecialActionType.CutGasSource, false);
                break;
            case SpecialActionType.CutOffCircuit:
                SetItemStatus((int)SpecialActionType.CutOffCircuit, true);
                SetItemStatus((int)SpecialActionType.CutGasSource, false);
                break;
            case SpecialActionType.CutGasSource:
                SetItemStatus((int)SpecialActionType.CutOffCircuit, false);
                SetItemStatus((int)SpecialActionType.CutGasSource, true);
                break;
        }

    }

    private void Start()
    {
        GameManager.UIUpdateAction += RefreshItemStatus;
    }

    #region DebugSector
    /*
    private void Start()
    {
        SetItemStatus((int)SpecialActionType.CutOffCircuit, true);
        SetItemStatus((int)SpecialActionType.CutGasSource, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetItemStatus((int)SpecialActionType.CutOffCircuit, !itemStatus[(int)SpecialActionType.CutOffCircuit]);
            SetItemStatus((int)SpecialActionType.CutGasSource, !itemStatus[(int)SpecialActionType.CutGasSource]);
        }
    }
    */
    #endregion
}
