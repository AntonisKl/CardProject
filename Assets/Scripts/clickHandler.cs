﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


/*
 *  Generic class used to identify User-GUI interaction.
 */

public class clickHandler : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,
    IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private static bool timesTwo = true;
    public Font m_Font;

    static int cardEntryOffset = 133;

    /*
     * Called the moment cursor is clicked.
     */
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag Begin");
    }

    /*
     *  Called while cursor is held down and moving.
     */
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
    }


    /*
     *  Called when cursor stops being held down.
     */
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag Ended");
    }

   /*
    *  Called when the user clicks.
    */
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject mainui = GameObject.Find("Main UI");
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals("mainscene"))
        {
            GameObject clickedObj = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
            if (clickedObj == null || clickedObj.name == "Main UI")
                clickedObj = eventData.pointerCurrentRaycast.gameObject;
            string clickedCard = clickedObj.name;
            Debug.Log("CLICKED OBJECT EXISTS: " + clickedObj);
            if (clickedCard.Equals("CardDisplaySample(Clone)"))
            {
                //Debug.Log("MANA:" + (clickedObj.GetComponent<CardDisplay>().manaCost.text));
                if (GameState.getActivePlayer().currentMana >= int.Parse(clickedObj.GetComponent<CardDisplay>().manaCost.text))
                {
                    GameState.getActivePlayer().playCard(GameState.getActivePlayer().handCards.IndexOf(clickedObj));

                    //Event system from here on out...
                    string s = clickedObj.GetComponent<CardDisplay>().cardName.ToString();
                    cardEventHandler.onMinionSummon(s);
                    GameState.getActivePlayer().decreaseCurrentMana(1);
                }
            }
        }
        else
        {
            if (deckbuilder.deckBuildActive == true)
            {
                
                if (Player.deck.Count < 30)
                {
                 
                    GameObject clickedObj = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
                    if (clickedObj == null)
                        clickedObj = eventData.pointerCurrentRaycast.gameObject;
                    string clickedCard = clickedObj.name;

                    Debug.Log("Clicked: " + clickedObj.transform.parent.name);
                    if (clickedCard.Equals("CardDisplaySample(Clone)") || clickedObj.transform.parent.name.Equals("CardDisplaySample(Clone)") || clickedCard.Equals("CardDisplaySpellSample(Clone)") || clickedObj.transform.parent.name.Equals("CardDisplaySpellSample(Clone)"))
                    {
                        Debug.Log(Player.deck.Count);
                        if (!Player.deck.Contains(clickedObj.GetComponent<CardDisplay>().id))
                        {
                            GameObject cardListEntry = new GameObject("cardListEntry");
                            cardListEntry.gameObject.tag = "cardEntry";
                            //cardListEntry = Instantiate(cardListEntry, GameObject.FindGameObjectWithTag("ListWithCards").transform);
                            cardListEntry.AddComponent<LayoutElement>();
                            cardListEntry.AddComponent<Text>();
                            //cardListEntry.GetComponent<Text>().font.name = "Arial";
                            cardListEntry.GetComponent<Text>().text = clickedObj.GetComponent<CardDisplay>().cardName.text;
                            cardListEntry.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                            cardListEntry.GetComponent<Text>().fontSize = 10;

                            //cardListEntry.transform.parent = GameObject.FindGameObjectWithTag("ListWithCards").transform;
                            cardListEntry = Instantiate(cardListEntry, GameObject.FindGameObjectWithTag("ListWithCards").transform);
                            cardEntryOffset -= 20;
                            cardListEntry.transform.localPosition = new Vector3(3.382453f, cardEntryOffset, 0);
                            ((RectTransform)cardListEntry.transform).sizeDelta = new Vector2(118.89f, 17.58f);
                            Player.deck.Add(clickedObj.GetComponent<CardDisplay>().id);
                            timesTwo = false;
                        }
                        else if(Player.deck.Contains(clickedObj.GetComponent<CardDisplay>().id) && timesTwo == false)
                        {
                            GameObject[] cardEntries = GameObject.FindGameObjectsWithTag("cardEntry");
                            foreach (GameObject card in cardEntries)
                            {
                                if (card.GetComponent<Text>().text.Equals(clickedObj.GetComponent<CardDisplay>().cardName.text))
                                {
                                    card.GetComponent<Text>().text = clickedObj.GetComponent<CardDisplay>().cardName.text + " x2";             
                                    timesTwo = true;
                                }
                            }
                            Player.deck.Add(clickedObj.GetComponent<CardDisplay>().id);
                        }
                    }
                }
            }
        }
    }

    public static void SaveDeck()
    {
        string deckCode = "";
        foreach(int cardId in Player.deck)
        {
            deckCode += cardId.ToString() + " ";
        }
        Debug.Log(deckCode);
        string path = "Assets/Prefabs/Resources/playerDecks.txt";
        StreamReader reader = new StreamReader(path, true);
        string allText = reader.ReadToEnd();
        reader.Close();

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(deckCode);
        writer.Close();

    }

   /*
    *  Called while cursor is stationary and held down.
    */
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Mouse Down: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

   /*
    *  Called when cursor enters the space allocated to the object attached to this script.
    */
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse Exit");
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals("deckbuilder"))
        {
            Debug.Log("Mouse Enter");
            Transform hoveredCardName = eventData.pointerCurrentRaycast.gameObject.transform.parent.Find("name");
            if(hoveredCardName==null)
                hoveredCardName = eventData.pointerCurrentRaycast.gameObject.transform.Find("name");

            Text textHoveredCardName = hoveredCardName.GetComponent<Text>();
            string s = textHoveredCardName.text.ToString();
            Text descText = GameObject.Find("description_text").GetComponent<Text>();
            int cardIndex = 0;
            for (; cardIndex < jsonparse.cards.Length; cardIndex++)
            {
                if (jsonparse.cards[cardIndex].card_name.Equals(s))
                    break;
            }
            Debug.Log("INDEX IS" + cardIndex);
            descText.text = jsonparse.cards[cardIndex].card_flavortext;
        }
    }

    /*
     *  Called when cursor exits the space allocated to the object attached to this script.
     */
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Mouse Exit");
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals("deckbuilder"))
        {
            Text descText = GameObject.Find("description_text").GetComponent<Text>();
            descText.text = "";
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Mouse Up");
    }
}
