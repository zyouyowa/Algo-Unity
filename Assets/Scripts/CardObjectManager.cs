﻿using UnityEngine;using System.Collections;using System.Collections.Generic;//GameFlowManagerと相互参照してるのどうなの???public class CardObjectManager : MonoBehaviour {    public GameObject cardPrefab;    public RectTransform deckPos;    public RectTransform drawedPos;    public RectTransform playerHandPos;    public RectTransform aiHandPos;    public HumanPlayerSelect ui;    public GameObject cardCanvas;    public float reverseTime = 1f;    private GameFlowManager flow;    private GameObject _nowDraw;    private bool _drawed;    private List<GameObject> aiHand;    private List<GameObject> playerHand;    private bool _isInitialized = false;	void Start () {        _drawed = false;        flow = GetComponent<GameFlowManager>();        if(!flow){            flow = GameObject.FindWithTag("GameFlowManager").GetComponent<GameFlowManager>();        }        aiHand = new List<GameObject>();        playerHand = new List<GameObject>();        _isInitialized = true;	}		void Update () {        	}    public IEnumerator InitHand(){        while(!_isInitialized) yield return new WaitForEndOfFrame();        foreach(var card in flow.playerHand){            yield return StartCoroutine(DrawCard(GameFlowManager.TurnStatus.PlayerTurn, card));            yield return StartCoroutine(IntoHand(GameFlowManager.TurnStatus.PlayerTurn, false));        }        foreach(var card in flow.aiHand){            yield return StartCoroutine(DrawCard(GameFlowManager.TurnStatus.AITurn, card));            yield return StartCoroutine(IntoHand(GameFlowManager.TurnStatus.AITurn, false));        }    }    public IEnumerator DrawCard(GameFlowManager.TurnStatus turnStatus, Card draw){        Debug.Log("test1.1");        _nowDraw = Instantiate(cardPrefab) as GameObject;        _nowDraw.GetComponent<RectTransform>().position = deckPos.position;        _nowDraw.GetComponent<RectTransform>().SetParent(cardCanvas.transform);        _nowDraw.GetComponent<RectTransform>().localScale = Vector3.one;        CardObject cardObj = _nowDraw.GetComponent<CardObject>();        cardObj.card = draw;        cardObj.ui = ui;        Debug.Log("test1.2");        yield return StartCoroutine(cardObj.Draw(turnStatus, deckPos, drawedPos));        _drawed = true;    }    //DrawCard使用後に使う    public IEnumerator IntoHand(GameFlowManager.TurnStatus turnStatus, bool isOpen){        //ドローが完了するまで待つ        while(!_drawed) yield return new WaitForEndOfFrame();        _drawed = false;        if(turnStatus == GameFlowManager.TurnStatus.PlayerTurn){            playerHand.Add(_nowDraw);            playerHand.Sort(CardObjectManager.CompareCard);            for(int i = 0; i < playerHand.Count; ++i){                playerHand[i].GetComponent<CardObject>().index = i;            }        } else {            aiHand.Add(_nowDraw);            aiHand.Sort(CardObjectManager.CompareCard);            for(int i = 0; i < aiHand.Count; ++i){                aiHand[i].GetComponent<CardObject>().index = i;            }        }        CardObject cardObj = _nowDraw.GetComponent<CardObject>();        if(turnStatus == GameFlowManager.TurnStatus.PlayerTurn){            yield return StartCoroutine(cardObj.IntoHand(drawedPos, playerHandPos, isOpen));        } else {            yield return StartCoroutine(cardObj.IntoHand(drawedPos, aiHandPos, isOpen));        }        //位置を整理する        if(turnStatus == GameFlowManager.TurnStatus.PlayerTurn){            foreach(var cgo in playerHand){                CardObject co = cgo.GetComponent<CardObject>();                float opening = 0.001f;                float width = cgo.GetComponent<RectTransform>().sizeDelta.x;                Vector3 origin = Vector3.zero;                if(playerHand.Count%2 == 0){                    //FIXME: Vector3.rightだとワールドのものが適用されているので危険                    origin = playerHandPos.position + Vector3.left * (playerHand.Count/2 * (width + opening) - width/2f - opening/2f);                } else {                    origin = playerHandPos.position + Vector3.left * (playerHand.Count/2 * (width + opening));                }                origin.x /= 2f;                StartCoroutine(co.MoveTo(origin + Vector3.right * (width/2f + opening) * co.index));            }        } else {            foreach(var cgo in aiHand){                CardObject co = cgo.GetComponent<CardObject>();                float opening = 0.001f;                float width = cgo.GetComponent<RectTransform>().sizeDelta.x;                Vector3 origin = Vector3.zero;                if(aiHand.Count%2 == 0){                    //FIXME: Vector3.rightだとワールドのものが適用されているので危険                    origin = aiHandPos.position + Vector3.right * (aiHand.Count/2 * (width + opening) - width/2f - opening/2f);                } else {                    origin = aiHandPos.position + Vector3.right * (aiHand.Count/2 * (width + opening));                }                origin.x /= 2f;                StartCoroutine(co.MoveTo(origin + Vector3.left * (width/2f + opening) * co.index));            }        }    }    public void OpenCard(GameFlowManager.TurnStatus turnStatus, int index){        if(turnStatus == GameFlowManager.TurnStatus.PlayerTurn){            var cardObj = aiHand[index].GetComponent<CardObject>();            cardObj.OpenCard();        } else {            var cardObj = playerHand[index].GetComponent<CardObject>();            cardObj.OpenCard();        }    }    public static int CompareCard(GameObject obj1, GameObject obj2){        //ここまずい?        Card c1 = obj1.GetComponent<CardObject>().card;        Card c2 = obj2.GetComponent<CardObject>().card;        if(c1 == c2) return 0;        else if(c1 < c2) return -1;        else if(c1 > c2) return 1;        else return 1; //ここはただのエラー回避...    }}