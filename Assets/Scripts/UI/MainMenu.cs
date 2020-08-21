using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    Vector3 downPos = new Vector3(0, 0, -1);
    Vector3 upPos = new Vector3(0, 200, -1);

    public void MoveUp()
    {
        transform.DOMove(upPos, 0.5f)
             .OnComplete(() =>
                GameManager.Instance.GameStart(1)
             );
    }

    public void MoveDown()
    {
        transform.DOMove(downPos, 0.5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MoveDown();
        }
        
    }
}
