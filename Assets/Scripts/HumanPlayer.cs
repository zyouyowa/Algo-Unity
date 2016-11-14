﻿using UnityEngine;using System.Collections;public class HumanPlayer : Player {    [HideInInspector]    public bool finishInput;    [HideInInspector]    public int forecastNumber;    [HideInInspector]    public int targetIndex;	void Start () {        finishInput = false;	}	    /*	void Update () {	    	}    */    public override IEnumerator Turn(){        Debug.Log("Player Turn");        Card draw = base.flow.DrawCard();        yield return StartCoroutine(base.cardObjManager.DrawCard(GameFlowManager.TurnStatus.PlayerTurn, draw));        base.flow.playerHand.Add(draw);        while(true){            while(!finishInput) yield return null;            finishInput = false;            //TODO: プレイヤーの言ったやつを含めてカードの予想をさせる            //base.flow.playerSaid.Add(new Card())            if(base.flow.aiHand[targetIndex].number == forecastNumber){                base.flow.aiHand[targetIndex].isOpen = true;                base.cardObjManager.OpenCard(GameFlowManager.TurnStatus.PlayerTurn, targetIndex);                if(GameFlowManager.CheckAllOpend(base.flow.aiHand)){                    base.flow.gameStatus = GameFlowManager.GameStatus.PlayerWin;                    yield break;                }            } else {                draw.isOpen = true;                yield return StartCoroutine(base.cardObjManager.IntoHand(GameFlowManager.TurnStatus.PlayerTurn, true));                base.flow.playerHand.Sort(GameFlowManager.CompareCard);                base.flow.aiKnown.Add(draw);                base.flow.gameStatus = GameFlowManager.GameStatus.Continue;                yield break;            }        }    }}