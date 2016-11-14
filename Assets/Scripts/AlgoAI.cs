﻿using UnityEngine;using System.Collections;using System.Collections.Generic;public class AlgoAI : Player {	void Start () {        //これはボタンか何かでスタートするようにする        //base.flow.gameStart = true;	}		void Update () {        	}    public override IEnumerator Turn(){        Debug.Log("AI Turn");        Card draw = base.flow.DrawCard();        Debug.Log("test1");        yield return StartCoroutine(base.cardObjManager.DrawCard(GameFlowManager.TurnStatus.AITurn, draw));        Debug.Log("test2");        base.flow.aiHand.Add(draw);        Debug.Log("test3");        while(true){            yield return null;            var aiForecasts = ForecasCard();            Debug.Log("test4");            int min_f = 0;            while(aiForecasts[min_f].Count == 0 ||                base.flow.playerHand[min_f].isOpen){                min_f++;            }            Debug.Log("test5");            for(int i = 0; i < aiForecasts.Count; ++i){                if(!base.flow.playerHand[i].isOpen){                    if(aiForecasts[i].Count < aiForecasts[i].Count){                        min_f = i;                    }                }            }            int choiceNumber = aiForecasts[min_f][Random.Range(0, aiForecasts[min_f].Count)];            Card choicedCard = new Card(base.flow.playerHand[min_f].color, choiceNumber);            base.flow.aiKnown.Add(choicedCard);            if(base.flow.playerHand[min_f].number == choiceNumber){                base.flow.playerHand[min_f].isOpen = true;                base.cardObjManager.OpenCard(GameFlowManager.TurnStatus.AITurn, min_f);                if(GameFlowManager.CheckAllOpend(base.flow.playerHand)){                    base.flow.gameStatus = GameFlowManager.GameStatus.AIWin;                    yield break;                }            } else {                draw.isOpen = true;                yield return StartCoroutine(base.cardObjManager.IntoHand(GameFlowManager.TurnStatus.AITurn, true));                base.flow.aiHand.Sort(GameFlowManager.CompareCard);                base.flow.gameStatus = GameFlowManager.GameStatus.Continue;                yield break;            }        }    }    private List<List<int>> ForecasCard(){        List<List<int>> aiForecasts = new List<List<int>>();        List<int> firsts = ForecastFirst();        List<int> lasts = ForecastLast(firsts);        for(int i = 0; i < base.flow.playerHand.Count; i++){            List<int> forecast = new List<int>();            for(int j = firsts[i]; j < lasts[i]+1; j++){                forecast.Add(j);            }            List<int> knownsSameColor = new List<int>();            foreach(Card card in base.flow.aiKnown){                if(card.color == base.flow.playerHand[i].color){                    knownsSameColor.Add(card.number);                }            }            List<int> saidSameColor = new List<int>();            foreach(Card card in base.flow.playerSaid){                if(card.color == base.flow.playerHand[i].color){                    knownsSameColor.Add(card.number);                }            }            foreach(int known in knownsSameColor){                forecast.Remove(known);            }            if(forecast.Count == 0){                foreach(int said in saidSameColor){                    if(!knownsSameColor.Contains(said)){                        forecast.Add(said);                    }                }                forecast.Sort();            }            //このあたり参照だから云々とかありそう            aiForecasts.Add(forecast);        }        return aiForecasts;    }    private List<int> ForecastFirst(){        List<int> firsts = new List<int>();        for(int i = 0; i < base.flow.playerHand.Count; ++i){            int first = 0;            if(i == 0){                firsts.Add(first);                continue;            }            if(base.flow.playerHand[i-1].isOpen){                first = flow.playerHand[i-1].number;            } else {                first = firsts[i-1];            }            if(base.flow.playerHand[i].color == Card.Color.White &&                 base.flow.playerHand[i-1].color == Card.Color.Black){                firsts.Add(first);            } else {                firsts.Add(first + 1);            }        }        return firsts;    }    private List<int> ForecastLast(List<int> firsts){        List<int> lasts = new List<int>();        for(int i = base.flow.playerHand.Count-1; i >= 0 ; --i){            int last = 11;            if(i == base.flow.playerHand.Count-1){                lasts.Add(last);                continue;            }            if(base.flow.playerHand[i+1].isOpen){                last = base.flow.playerHand[i+1].number;            } else {                last = lasts[base.flow.playerHand.Count-1 - i - 1];            }            if(base.flow.playerHand[i].color == Card.Color.Black &&                 base.flow.playerHand[i+1].color == Card.Color.White){                lasts.Add(last);            } else {                lasts.Add(last - 1);            }        }        lasts.Reverse();        return lasts;    }}