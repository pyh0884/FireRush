using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OverMenuPanel : MonoBehaviour
{
    Vector3 OutPos = new Vector3(260f, 0f, 0f);
    Vector3 InPos = new Vector3(0f, 0f, 0f);

    public void MoveIn()
    {
        transform.DOMove(new Vector3(InPos.x,transform.position.y,transform.position.z), 0.5f);
    }

    public void MoveOut()
    {
        transform.DOMove(new Vector3(OutPos.x, transform.position.y, transform.position.z), 0.5f);
    }
}
