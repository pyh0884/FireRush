using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverMenu : MonoBehaviour
{
    [SerializeField] OverMenuPanel panel;
    [SerializeField] GameObject obj_win;
    [SerializeField] GameObject obj_lost;

    public void GameWin()
    {
        obj_win.SetActive(true);
        obj_lost.SetActive(false);
        panel.MoveIn();
    }

    public void GameLost()
    {
        obj_win.SetActive(false);
        obj_lost.SetActive(true);
        panel.MoveIn();
    }

    public void ClosePanel()
    {
        obj_win.SetActive(false);
        obj_lost.SetActive(false);
        panel.MoveOut();
    }

    public void Restart()
    {
        ClosePanel();
        GameManager.Instance.GameStart(1);

    }

    private void Start()
    {
        GameManager.GameIsLostAction += GameLost;
        GameManager.GameIsWinAction += GameWin;
    }

    private void OnDestroy()
    {
        GameManager.GameIsLostAction -= GameLost;
        GameManager.GameIsWinAction -= GameWin;
    }
}
