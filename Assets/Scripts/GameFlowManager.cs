﻿using UnityEngine;using System.Collections;using System.Collections.Generic;using UnityEngine.UI;public class GameFlowManager : MonoBehaviour {    public enum GameStatus{        Continue,        PlayerWin,        AIWin    }    public enum TurnStatus{        PlayerTurn,        AITurn    }    public List<Card> deck;         //ドロー時はここからランダムに要素を取り出す    public List<Card> playerHand;    public List<Card> aiHand;    public List<Card> playerSaid;    public List<Card> aiSaid;    public List<Card> aiKnown;    public Player human;    public Player ai;    public GameObject winnerShower;    [HideInInspector]    public bool gameStart;//aiの初期化終了後にtrueになる    [HideInInspector]    public GameStatus gameStatus;    [HideInInspector]    public CardObjectManager cardObjManager;    void Start () {        winnerShower.SetActive(false);        //init deck        deck = new List<Card>();        playerHand = new List<Card>();        aiHand = new List<Card>();        playerSaid = new List<Card>();        aiSaid = new List<Card>();        aiKnown = new List<Card>();        gameStatus = GameStatus.Continue;        if(cardObjManager == null)            cardObjManager = GetComponent<CardObjectManager>();        gameStart = false;        Card.Color[] cardColors = {Card.Color.Black, Card.Color.White};        foreach(var cardColor in cardColors){            for(int i = 0; i <= 11; i++){                deck.Add(new Card(cardColor, i));            }        }        //init hands        for(int i = 0; i < 4; i++){            playerHand.Add(DrawCard());            aiHand.Add(DrawCard());        }        playerHand.Sort(CompareCard);        aiHand.Sort(CompareCard);        /*foreach(var c in playerHand){            Debug.LogFormat("{0}{1}", c.color, c.number);        }*/        aiKnown.AddRange(aiHand);        IEnumerator flow = GameFlow();        Debug.Log("Init Game!");        StartCoroutine(flow);	}    IEnumerator GameFlow(){        //何かを待つ        while(!gameStart) yield return null;        Debug.Log("Game Start!");         yield return StartCoroutine(cardObjManager.InitHand());        TurnStatus now = (Random.Range(0, 1) == 0)? TurnStatus.PlayerTurn : TurnStatus.AITurn;        while(true){            if(now == TurnStatus.PlayerTurn){                yield return StartCoroutine(human.Turn());            } else {                yield return StartCoroutine(ai.Turn());            }            if(gameStatus != GameStatus.Continue){                Debug.Log("Game Finish!");                break;            }            now = (now == TurnStatus.PlayerTurn)? TurnStatus.AITurn : TurnStatus.PlayerTurn;        }        winnerShower.SetActive(true);        var txt = winnerShower.GetComponentInChildren<Text>();        if(gameStatus == GameStatus.AIWin){            txt.text = "You Lose...";        } else {            txt.text = "You Win!";        }    }    public static int CompareCard(Card c1, Card c2){        if(c1 == c2) return 0;        else if(c1 < c2) return -1;        else if(c1 > c2) return 1;        else return 1; //ここはただのエラー回避...    }    public Card DrawCard(){        int index = Random.Range(0, deck.Count);        Card card = new Card(deck[index]);        deck.RemoveAt(index);        return card;    }    public void StartGame(){        gameStart = true;    }    public static bool CheckAllOpend(List<Card> hand){        bool allOpend = true;        foreach(Card card in hand){            allOpend = allOpend && card.isOpen;        }        return allOpend;    }}