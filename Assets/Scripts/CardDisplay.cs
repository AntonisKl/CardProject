﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour {
    
    public GameObject card;
    public static List<string> cardKeywords; 
    private bool isInstantiated = false;

    public Text cardName;
    public Text description;
    public Text manaCost;
    public Text type;

    public Image artwork;
    public Image element;

    public Text power;
    public int id;


    void Start()
    {
        if (this.name.Equals("CardDisplaySample(Clone)"))                                   //Needs checking or the cloned object will clone again.
        {
            isInstantiated = true;
        }
    }

    /*
     * Function used to clone the card prefab and place it in the main UI.
     * Arguments: Vector3 Coordinates(where to instantiate the card)
     */
    public GameObject initializeCard(float x, float y, float z, int cardId)                                                              
    {

            /*This prints the card*/
            cardId--;
            GameObject mainui = GameObject.Find("Main UI");
            if(jsonparse.cards[cardId].card_type.Equals("Minion"))
                card = (GameObject)Instantiate(Resources.Load("CardDisplaySample"));
            else
                card = (GameObject)Instantiate(Resources.Load("CardDisplaySpellSample"));
            card.transform.SetParent(mainui.transform, false);
            card.GetComponent<CardDisplay>().DisplayCard(cardId);
            card.transform.localPosition = new Vector3(x, y, z);
            return card;
    }

    /*
     * Main function linking the json attributes to the GUI Elements of the Card object.
     */
    public void DisplayCard (int cardId)
    {
        id = cardId;
        
        cardName.GetComponent<Text>();
        cardName.text = jsonparse.cards[cardId].card_name;
        description.text = jsonparse.cards[cardId].card_text;
        manaCost.text = jsonparse.cards[cardId].card_manacost.ToString();
        type.text = jsonparse.cards[cardId].card_type.ToString();
        string isMinion = jsonparse.cards[cardId].card_type.ToString();
        if (isMinion.Equals("Minion"))
        {
            power.text = jsonparse.cards[cardId].card_actionpoints.ToString();
        }
        string attribute = jsonparse.cards[cardId].card_attribute.ToString();
        if (attribute.Equals("Fire"))
        {
            element.sprite = Resources.Load("fire", typeof(Sprite)) as Sprite;
        }
        else if (attribute.Equals("Light"))
        {
            element.sprite = Resources.Load("light", typeof(Sprite)) as Sprite;
        }
        else if (attribute.Equals("Dark"))
        {
            element.sprite = Resources.Load("dark", typeof(Sprite)) as Sprite;
        }
        //TODO: Other attributes...
        artwork.sprite = Resources.Load(jsonparse.cards[cardId].card_image.ToString(), typeof(Sprite)) as Sprite;
    }

    //public void OnMouseEnter()
    //{
    //    Debug.Log("HIIII");
    //    Scene currentScene = SceneManager.GetActiveScene();
    //    if (currentScene.name.Equals("deckbuilder")){
    //        Text descText = GameObject.Find("description_text").GetComponent<Text>();
    //        descText.text = jsonparse.cards[id].card_flavortext;
    //    }
    //    else{
    //        gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    //        gameObject.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 75, 0);
    //        gameObject.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
    //    }
    //}


    //public void OnMouseExit()
    //{
    //    Scene currentScene = SceneManager.GetActiveScene();
    //    if (currentScene.name.Equals("deckbuilder"))
    //    {
    //        Text descText = GameObject.Find("description_text").GetComponent<Text>();
    //        descText.text = "";
    //    }
    //    else
    //    {
    //        gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
    //        gameObject.transform.localPosition = gameObject.transform.localPosition - new Vector3(0, 75, 0);
    //        gameObject.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
    //    }
    //}

}

