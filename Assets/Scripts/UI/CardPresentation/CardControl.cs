using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Add this as the UI component
/// Other will use this class to set the card in the scene
/// </summary>
public class CardControl : MonoBehaviour
{
    [SerializeField] GameObject[] cardModel;
    GameObject newCard;
    CardBehavior cardBehavior;

    public GameObject CardGeneration(ResourcePackage itemsCounter) //use this function to generate a card prefab
    {

        int cardModelIndex = 0;
        newCard = cardModel[cardModelIndex]; //set a default value with wrong icon

        if (itemsCounter.SpecialAction != SpecialActionType.None)  //is a special item card
        {
            cardModelIndex = 0;
            newCard = cardModel[cardModelIndex];    //select card model

            return newCard;
        }
        else  //is a normal card
        {
            if (itemsCounter.Sum > 5 || itemsCounter.Sum < 1)
            {
                DebugExtension.DebugText("Total item number is illegal, sum is {0}", itemsCounter.Sum);
                return null;

            }
            else
            {
                cardModelIndex = itemsCounter.Sum - 1;  //index start from 0
                newCard = cardModel[cardModelIndex];    //select card model

                return newCard;

            }
        }
    }

    public void CardIconGeneration(ref CardBehavior card, ResourcePackage itemsCounter) //use this function to generate a card prefab
    {
        

        if (itemsCounter.SpecialAction != SpecialActionType.None)  //is a special item card
        {
            //DebugExtension.DebugText("setting icons as SpecialItem Card");
            card.SetSpecialIcons(0, (int)itemsCounter.SpecialAction);   //set the card 0 icon
        }
        else  //is a normal card
        {
            if (itemsCounter.Sum > 5 || itemsCounter.Sum < 1)
            {
                DebugExtension.DebugText("Total item number is illegal, sum is {0}", itemsCounter.Sum);
                return;

            }
            else
            {
                //DebugExtension.DebugText("setting icons as normal Card");
                int[] randomIcons = RandomIndex(itemsCounter.Sum);  //generate icon set sequence
                List<CardAttribute.ITEM_TYPE> items = GenerateItems(itemsCounter);//generate item set sequence
                for (int i = 0; i < randomIcons.Length; i++)    //set the card icon
                {
                    card.SetIcons(randomIcons[i], (int)items[i]);
                    //DebugExtension.DebugText("current setting icon item index is : {0}", (int)items[i]);
                }
            }
        }
    }

    public int[] RandomIndex(int length)
    {
        int[] indexs = new int[length];
        for (int i= 0; i < length; i++) //generate a normal order index sequence
        {
            indexs[i] = i;
        }

        for (int i = 0; i < length; i++)    //each loop generate a random index to switch
        {
            int temp = indexs[i];
            int randomIndex = Random.Range(0, length);
            indexs[i] = indexs[randomIndex];
            indexs[randomIndex] = temp;
        }
        //DebugExtension.DebugText("The random icon index are below");
        for (int i = 0; i < length; i++)
        {
            //DebugExtension.DebugText("the NO.{0} index is: {1}", i, indexs[i]);
        }
        return indexs;
    }

    public List<CardAttribute.ITEM_TYPE> GenerateItems(ResourcePackage resource)
    {
        List<CardAttribute.ITEM_TYPE> itemIndexs = new List<CardAttribute.ITEM_TYPE>();
        int index = 0;
        int WaterCount = resource.Water;
        int SandCount = resource.Sand;
        int FoamCount = resource.Foam;

        while (WaterCount > 0)
        {
            itemIndexs.Add(CardAttribute.ITEM_TYPE.WATER);
            WaterCount--;
        }
        while (SandCount > 0)
        {
            itemIndexs.Add(CardAttribute.ITEM_TYPE.SAND);
            SandCount--;
        }
        while (FoamCount > 0)
        {
            itemIndexs.Add(CardAttribute.ITEM_TYPE.FOAM);
            FoamCount--;
        }
        //DebugExtension.DebugText("The generate items are below");
        for (int i = 0; i < itemIndexs.Count; i++)
        {
            //DebugExtension.DebugText("the NO.{0} item is: {1}", i, itemIndexs[i]);
        }
        return itemIndexs;
    }
}
