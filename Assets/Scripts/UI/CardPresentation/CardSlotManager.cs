using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
/// <summary>
/// Two Main function for card spawning
/// 
/// chage a card on target slot 
/// ChangeCard(int cardSlotIndex, ResourcePackage itemCounts) 
/// 
/// spawn a card on target slot
/// SpawnCard(int cardSlotIndex, ResourcePackage itemCounts) 
/// 
/// </summary>
public class CardSlotManager : MonoBehaviour
{
    CardControl cardControl;

    public CardSlot[] cardSlots; //four card slots in the scene

    AudioSystem audio;
    [SerializeField] AudioClip[] drawCardAudioClip;

    int _lastSelectedSlotIndex;
    int lastSelectedSlotIndex
    {
        get
        {
            return _lastSelectedSlotIndex;
        }
    }

    public static event Action CardSlotSelected;

    public void CardRegister(int cardSlotIndex, GameObject card)
    {
        cardSlots[cardSlotIndex].SetCurrentCard(card);
        //do animation
        cardSlots[cardSlotIndex].currentCard.GetComponent<Rigidbody2D>().DOMove(cardSlots[cardSlotIndex].t_cardPos.position, 0.3f, false)

            .OnComplete(() => {
                audio.PlayAudio(drawCardAudioClip);
                cardSlots[cardSlotIndex].SetInteractable(true);  //player can use that slot again
            });
    }

    public void CardUnRegister(int cardSlotIndex)
    {
        //prevent furthur action of that slot
        cardSlots[cardSlotIndex].SetInteractable(false);

        GameObject card = cardSlots[cardSlotIndex].currentCard;
        cardSlots[cardSlotIndex].currentCard.GetComponent<Rigidbody2D>().DOMove(cardSlots[cardSlotIndex].t_cardEndPos.position, 0.3f, false)
            .OnComplete(()=>
                Destroy(card)
             );
        //set self destroy before unregister that card

        //cardSlots[cardSlotIndex].SetCurrentCard();


    }

    public void SpawnCard(int cardSlotIndex, ResourcePackage itemCounts) //spawn a card on target slot
    {
        GameObject newCard = GameObject.Instantiate(cardControl.CardGeneration(itemCounts), cardSlots[cardSlotIndex].t_cardSpawnPos.position, cardSlots[cardSlotIndex].t_cardSpawnPos.rotation, cardSlots[cardSlotIndex].transform);
        CardBehavior cardBehavior = newCard.GetComponent<CardBehavior>();
        cardControl.CardIconGeneration(ref cardBehavior, itemCounts);
        CardRegister(cardSlotIndex, newCard);
    }


    public void ChangeCard(int cardSlotIndex, ResourcePackage itemCounts) //chage a card on target slot
    {
        if (cardSlots[cardSlotIndex].currentCard != null)
        {
            CardUnRegister(cardSlotIndex);
            GameObject newCard = GameObject.Instantiate(cardControl.CardGeneration(itemCounts), cardSlots[cardSlotIndex].t_cardSpawnPos.position, cardSlots[cardSlotIndex].t_cardSpawnPos.rotation, cardSlots[cardSlotIndex].transform);
            CardBehavior cardBehavior = newCard.GetComponent<CardBehavior>();
            cardControl.CardIconGeneration(ref cardBehavior, itemCounts);
            CardRegister(cardSlotIndex, newCard);
        }
        else
        {
            SpawnCard(cardSlotIndex, itemCounts);
        }

        
    }

    public void CardSelect(int slotIndex)
    {
     
        DebugExtension.DebugText("{0} is selected, prepar for resouce calculation", slotIndex);
        _lastSelectedSlotIndex = slotIndex; //refresh the last Selected Slot Index

        GameManager.Instance.CardSelected(slotIndex); //send message to GameManager

        if (CardSlotSelected != null)//call the calculation class
        {
            CardSlotSelected();
        }
        
    }

    private void Start()
    {
        cardControl = GetComponent<CardControl>();
        audio = GetComponent<AudioSystem>();

    }
}
