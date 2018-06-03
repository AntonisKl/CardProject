﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardEventHandler : MonoBehaviour {

    public delegate void minionEventHandler(string minionName);

    public static event minionEventHandler onSummon;
    public static event minionEventHandler onDeath;

    /*
     * Function used to handle the on Summon effect
     */
    public static void onMinionSummon(string minionName)
    {
        cardEffects.instance.setUpDelegate(minionName);

        if (GameState.getActivePlayer().selectedCard.GetComponent<Card>().type.text.Equals("Spell"))
        {
            foreach ( Pair<int, int> node in GameState.getActivePlayer().availableNodesForSummon)
                GameState.boardTable[node.First, node.Second].GetComponent<nodeInfo>().makeInactive();
            GameState.getActivePlayer().availableNodesForSummon = null;
        }
        onSummon(minionName);

        if (GameState.getActivePlayer().selectedCard.GetComponent<Card>().type.text.Equals("Minion"))
        {
            Destroy(GameState.getActivePlayer().selectedCard);
            GameState.getActivePlayer().handCards.RemoveAt(GameState.getActivePlayer().selectedCardIndex);
        }
    }

    /*
     * Function used to handle the on Death effect
     */
    public static void onMinionDeath(string minionName)
    {
        onDeath(minionName);
    }

}
