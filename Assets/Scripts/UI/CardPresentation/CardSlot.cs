using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardSlot : InteractiveObject
{
    SpriteRenderer spriteRenderer;

    public Transform t_cardSpawnPos;
    public Transform t_cardPos;
    public Transform t_cardEndPos;
    GameObject _currentCard;
    public GameObject currentCard
    {
        get
        {
            return _currentCard;
        }
    }

    public UnityEvent SelectSlot;


    /*public override void Tap(float tapPosX, float tapPosY)
    {
        base.Tap(tapPosX, tapPosY);

        SelectSlot.Invoke();
    }*/

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        SelectSlot.Invoke();
    }

    public void SetCurrentCard(GameObject card)
    {
        _currentCard = card;
    }

    public void SetCurrentCard()
    {
        _currentCard = null;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.enabled = false;
    }
}
