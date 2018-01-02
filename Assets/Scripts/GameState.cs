﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
	//This is the class to handle board, check board conditions (can monster move/summon) where it wants etc
	//Also initiates and controls game flow (who plays, time etc).
	//all vars are static because we will only have one game running anyway

	static public Player[] players;
	static public int numOfPlayers;
	static public int activePlayerIndex;
	static private float moveTime = 10.0f;
	static public float currentMoveTime;
	static public GameObject[,] boardTable;
	//general info regarding board
	static public int dimensionX=7, dimensionY=11;
	static private float xspacing = 1.2f, yspacing = 1.2f; //offset for board view

	// Use this for initialization
	void Start () {
		boardTable = createBoard (dimensionX, dimensionY);
		setPlayers (2);
		moveTime = 10.0f;
		startGame ();
	}

	// Update is called once per frame
	void Update () {
		currentMoveTime -= Time.deltaTime;
		if (currentMoveTime <= 0) { //end of turn
			nextPlayerTurn();
		}
		//to get seconds in int value use:
		//Mathf.Ceil(currentMoveTime);
		//Debug.Log("Playing " + players[activePlayerIndex].pName + " time " + Mathf.Ceil(currentMoveTime));
	}

	public GameObject[,] createBoard(int sizex, int sizey) {
		GameObject[,] myTable = new GameObject[sizex,sizey];
		int i, j;
		float offsetx = 0.0f, offsety = 0.0f;
		GameObject node = ((GameObject)Resources.Load ("node"));
		for (i = 0; i < sizey; i++) {
			for (j = 0; j < sizex; j++) {
				GameObject cube = Instantiate (node);
				cube.transform.SetParent (GameObject.Find("Board").transform, false);
				cube.transform.localPosition = new Vector3 (-yspacing+offsety, 0.0f, 0+offsetx);
				cube.GetComponent<nodeInfo> ().isFree = true;
				cube.name = "node " + (j+1) + "," + (i+1);
				myTable [j,i] = cube;
				offsety += yspacing;
			}
			offsetx += xspacing;
			offsety = 0;
		}
		return myTable;
	}

	public void setPlayers(int playersNum) { //will probably change to array of Player later on
		numOfPlayers = playersNum;
		players = new Player[numOfPlayers];
		int i = 0;
		for (; i < numOfPlayers; i++) {
			players [i] = new Player (i+1, "Player" + (i + 1));
		}
	}

	private void startGame() {
		int i = 0;
		for (; i < numOfPlayers; i++) {
			players [i].InstantiateHero ();
		}
		activePlayerIndex = 0;
		currentMoveTime = moveTime;
		players [activePlayerIndex].switchPlayState ();
	}

	static public void nextPlayerTurn() {
		players [activePlayerIndex].switchPlayState ();
		activePlayerIndex = (activePlayerIndex + 1) % numOfPlayers;
		players [activePlayerIndex].switchPlayState ();
		currentMoveTime = moveTime; //reset time
		//add Player functions to start turn properly here, using players[activePlayerIndex]
	}

	static public Player getActivePlayer() {
		return players [activePlayerIndex];
	}

	static public GameObject summonMonster(GameObject myMonster, List< Pair<int,int> > proposedPos) {
		foreach (Pair<int,int> pair in proposedPos) {
			if (boardTable [pair.First, pair.Second].GetComponent<nodeInfo> ().isFree == false)
				return null;
		}
		//call any animations etc and instantiate object relative to board
		foreach (Pair<int,int> pair in proposedPos) {
			boardTable [pair.First, pair.Second].GetComponent<nodeInfo> ().isFree = false; //allocating the board space
		}
		//update monster position
		GameObject instantiated = Instantiate(myMonster, fixPositionRelativeToBoard(myMonster, proposedPos), new Quaternion(0,0,0,0));
		instantiated.GetComponent<monsterInfo>().setPosition(proposedPos);
		return instantiated;
	}

	static private Vector3 fixPositionRelativeToBoard(GameObject myObj, List< Pair<int,int> > pos) {
		//get average of positions in list, centering the object
		Pair<float,float> avgpos = new Pair<float,float>(0f,0f);
		foreach (Pair<int,int> pair in pos) {
			avgpos.First += pair.First;
			avgpos.Second += pair.Second;
		}
		avgpos.First /= pos.Count;
		avgpos.Second /= pos.Count;
		//move relative to node [0,0], based on avg and offsets
		return boardTable[0,0].transform.position + new Vector3(12*avgpos.First, 5, 12*avgpos.Second);
	}

	static public Dictionary<Pair<int,int>, int> availableMonsterMovements(GameObject monster) {
		//monster param to get the movspeed and/or additional movement effects (flying etc)
		Dictionary<Pair<int,int>, int> availableMoves = new Dictionary<Pair<int,int>, int> ();
		List<Pair<int,int>> startPos = monster.GetComponent<monsterInfo> ().coords;
		List<Pair<int,int>> goalPos = new List<Pair<int,int>>();
		int maxmoves = monster.GetComponent<monsterInfo>().movspeed;
		int i, j, temp=1;
		for (i = -maxmoves; i <= maxmoves; i++) {
			for (j = -maxmoves+Mathf.Abs(i); j <= maxmoves-Mathf.Abs(i); j++) {
				goalPos.Clear ();
				if (i == 0 && j == 0)
					continue;
				foreach (Pair<int,int> startpair in startPos) {
					if (startpair.First + i >= 0 && startpair.First + i < dimensionX && startpair.Second + j >= 0 && startpair.Second + j < dimensionY)
						goalPos.Add (new Pair<int,int> (startpair.First + i, startpair.Second + j));
					else {
						goalPos.Clear ();
						break;
					}
				}
				if (true/*DFS (startPos, goalPos, maxmoves)*/) {
					//Debug.Log ("SOLUTION " + temp);
					foreach (Pair<int,int> pair in goalPos) {
							//Debug.Log ("Solution " + temp + " contains " + pair.First + "," + pair.Second);
					}
				}
				temp++;
				foreach (Pair<int,int> pair in goalPos) {
					if (!availableMoves.ContainsKey (pair)) {
						availableMoves.Add (pair, 1);
					}
				}
			}
		}
		return availableMoves;
	}

	static private bool DFS(List<Pair<int,int>> curPos, List<Pair<int,int>> goalPos, int maxmoves) {
		List<Pair<int,int>> node = new List<Pair<int,int>> ();
		Stack frontier = new Stack ();
		frontier.Push (new Pair <List<Pair<int,int>>, int> (curPos, 0)); //stack stores the list of position nodes and the moves that were needed for that
		Dictionary<List<Pair<int,int>>, int> explored = new Dictionary<List<Pair<int,int>>, int> ();
		while (frontier.Count > 0) {
			node = ((Pair <List<Pair<int,int>>, int>)frontier.Peek ()).First;
			int moves = ((Pair <List<Pair<int,int>>, int>)frontier.Peek ()).Second;
			if (CompareLists<Pair<int,int>> (goalPos, node)) {
				return true;
			}
			if (moves >= maxmoves) {
				continue;
			}
			frontier.Pop ();
			explored.Add (node, moves);
			int i, j;
			for (i = -1; i <= 1; i++) {
				for (j = -1 + Mathf.Abs (i); j < 1 - Mathf.Abs (i); j++) {
					List<Pair<int,int>> child = new List<Pair<int,int>> ();
					foreach (Pair<int,int> startpair in node) {
						if (startpair.First+i >= 0 && startpair.First+i < dimensionX && startpair.Second+j >= 0 && startpair.Second+j < dimensionY && boardTable [startpair.First + i, startpair.Second + j].GetComponent<nodeInfo> ().isFree) {
							child.Add (new Pair<int,int> (startpair.First + i, startpair.Second + j));
						} else {
							child.Clear ();
							break;
						}
					}
					if (child.Count>0 && !explored.ContainsKey(child)) {
						frontier.Push (new Pair <List<Pair<int,int>>, int> (child, moves+1));
					}
				}
			}
		}
		return false;
	}

	//utility O(2n) function to compare 2 lists
	static private bool CompareLists<T>(List<T> aListA, List<T> aListB)
	{
		if (aListA == null || aListB == null || aListA.Count != aListB.Count)
			return false;
		if (aListA.Count == 0)
			return true;
		Dictionary<T, int> lookUp = new Dictionary<T, int>();
		// create index for the first list
		for(int i = 0; i < aListA.Count; i++)
		{
			int count = 0;
			if (!lookUp.TryGetValue(aListA[i], out count))
			{
				lookUp.Add(aListA[i], 1);
				continue;
			}
			lookUp[aListA[i]] = count + 1;
		}
		for (int i = 0; i < aListB.Count; i++)
		{
			int count = 0;
			if (!lookUp.TryGetValue(aListB[i], out count))
			{
				// early exit as the current value in B doesn't exist in the lookUp (and not in ListA)
				return false;
			}
			count--;
			if (count <= 0)
				lookUp.Remove(aListB[i]);
			else
				lookUp[aListB[i]] = count;
		}
		// if there are remaining elements in the lookUp, that means ListA contains elements that do not exist in ListB
		return lookUp.Count == 0;
	}
}
