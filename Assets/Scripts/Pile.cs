using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Pile : Zone {

    public List<Card> cards = new List<Card>();
    public string name;

    public Pile(string name_in = "")
    {
        name = name_in;
    }

    public virtual string GetName()
    {
        return name;
    }

    public void DropCard(Card c)
    {
        if(name == "")
        {
            Debug.Log("Unnamed Pile dropped");
        }
        else
        {
            Debug.Log(name + " dropped");
        }
        cards.Remove(c);
        Assert.IsFalse(cards.Contains(c));
    }

    public void AddCard(Card c)
    {
        if(name == "")
        {
            Debug.Log("Unnamed Pile gained");
        }
        else
        {
            Debug.Log(name + " gained");
        }
        cards.Add(c);
        if(c.p_card != null)
        {
            c.p_card.BreakPCard();
        }
    }

    public override string ToString()
    {
        return name;
    }
}
