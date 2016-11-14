﻿using System.Collections;using System;public class Card{    public enum Color{        Black = 0,        White = 1    }    private int _number;    public int number{        get{ return _number; }        set{             if(value < 0){                _number = 0;            } else if(11 < value){                _number = 11;            } else {                _number = value;            }        }    }    public Color color{ get; set; }    public bool isOpen{ get; set; }    public Card(Card.Color color, int number){        this.color = color;        this.number = number;        this.isOpen = false;    }    public Card(Card card){        this.color = card.color;        this.number = card.number;        this.isOpen = card.isOpen;    }    public static bool operator ==(Card c1, Card c2){        return (c1.color == c2.color) && (c1.number == c2.number);    }    public static bool operator !=(Card c1, Card c2){        return !(c1 == c2);    }    public static bool operator < (Card c1, Card c2){        if(c1.number == c2.number){            //同じ数ならば黒<白            return c1.color < c2.color;        }        return c1.number < c2.number;    }    public static bool operator > (Card c1, Card c2){        if(c1.number == c2.number){            //同じ数ならば黒<白            return c1.color > c2.color;        }        return c1.number > c2.number;    }    public static bool operator <= (Card c1, Card c2){        if(c1.number == c2.number){            return c1.color <= c2.color;        }        return c1.number <= c2.number;    }    public static bool operator >= (Card c1, Card c2){        if(c1.number == c2.number){            //同じ数ならば黒<白            return c1.color >= c2.color;        }        return c1.number >= c2.number;    }}