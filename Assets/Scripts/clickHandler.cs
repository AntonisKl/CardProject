﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class clickHandler : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,
    IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Font m_Font;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Begin");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Ended");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals("mainscene"))
        {
            string clickedCard = eventData.pointerCurrentRaycast.gameObject.transform.parent.name;
            Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.transform.parent.name);
            if (clickedCard.Equals("CardDisplaySample(Clone)"))
            {
                GameObject mainui = GameObject.Find("Main UI");
                GameObject creatureText = new GameObject("Summon_Creature_Text");
                creatureText.transform.SetParent(mainui.transform);
                Text newtext = creatureText.AddComponent<Text>();
                newtext.font = m_Font;
                newtext.text = "Choose where to summon the creature";
                newtext.color = UnityEngine.Color.black;
                newtext.transform.localPosition = new Vector3(100, 52, 0);
            }
        }
        else
        {
            Debug.Log("Hi");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Mouse Down: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse Exit");
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals("deckbuilder"))
        {
            Debug.Log("Mouse Enter");
            Transform hoveredCardName = eventData.pointerCurrentRaycast.gameObject.transform.parent.Find("name");
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

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse Exit");
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals("deckbuilder"))
        {
            Text descText = GameObject.Find("description_text").GetComponent<Text>();
            descText.text = "";
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Mouse Up");
    }
}
