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

	static public bool allocateBoardPosition(List<Pair<int,int>> allocPos) {
		foreach (Pair<int,int> pair in allocPos) {
			if (boardTable [pair.First, pair.Second].GetComponent<nodeInfo> ().isFree == false)
				return false;
		}
		foreach (Pair<int,int> pair in allocPos) {
			boardTable [pair.First, pair.Second].GetComponent<nodeInfo> ().isFree = false; //allocating the board space
		}
		return true;
	}

	static public Vector3 getPositionRelativeToBoard(List< Pair<int,int> > pos) {
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
					if (startpair.First + i >= 0 && startpair.First + i < dimensionX && startpair.Second + j >= 0 && startpair.Second + j < dimensionY && boardTable[startpair.First+i,startpair.Second+j].GetComponent<nodeInfo>().isFree)
						goalPos.Add (new Pair<int,int> (startpair.First + i, startpair.Second + j));
					else {
						goalPos.Clear ();
						break;
					}
				}
				if (goalPos.Count > 0) {
					if (DFS (startPos, goalPos, maxmoves)) {
						foreach (Pair<int,int> pair in goalPos) {
							if (!availableMoves.ContainsKey (pair)) {
								availableMoves.Add (pair, 1);
							}
						}
					}
				}
				temp++;
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
			frontier.Pop ();
			Debug.Log (goalPos [0].First + "," + goalPos [0].Second + " = " + node [0].First + "," + node [0].Second);
			if (ComparePairLists (goalPos, node)) {
				Debug.Log ("YES");
				return true;
			}
			if (moves > maxmoves) {
				continue;
			}
			explored.Add (node, moves);
			int i, j;
			for (i = -1; i <= 1; i++) {
				for (j = -1 + Mathf.Abs (i); j <= 1 - Mathf.Abs (i); j++) {
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

	//utility O(n^2) function to compare 2 lists of pairs
	//we need better complexity algorithm for that
	static private bool ComparePairLists(List<Pair<int,int>> aListA, List<Pair<int,int>> aListB)
	{
		if (aListA == null || aListB == null || aListA.Count != aListB.Count)
			return false;
		if (aListA.Count == 0)
			return true;
		for (int i = 0; i < aListA.Count; i++) {
			bool flag = false;
			for (int j = 0; j < aListB.Count; j++) {
				if (aListA [i].First == aListB [j].First && aListA [i].Second == aListB [j].Second) {
					flag = true;
					aListB.RemoveAt (j); //we remove the common element to avoid unneseccary checks later
					break;
				}
			}
			if (!flag)
				return false;
		}
		return true;
	}
}
