using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffledPile : Zone {

    public string name;
    public ShuffledPile(string name_in)
    {
        name = name_in;
    }
    public List<Card> cards = new List<Card>();
	public virtual string GetName()
	{
		return name;
	}

    public void DropCard(Card c)
    {
        cards.Remove(c);
    }

    public PhysicalCard DrawCard(Zone target)
    {
        PhysicalCard createdCard = PhysicalCardFactory.CreateCard(cards[0]);
        cards[0].MoveTo(target);
        return createdCard;
    }

    public void AddCard(Card c)
    {
        int index = Random.Range(0, cards.Count + 1);
        cards.Insert(index, c);
        if(c.p_card != null)
        {
            c.p_card.BreakPCard();
        }
    }

    public void AddAtPosition(Card c, int pos)
    {
        int index = Random.Range(0, cards.Count + 1);
        cards.Insert(index, c);
        if(c.p_card != null)
        {
            c.p_card.BreakPCard();
        }

    }

    public int GetCount()
    {
        return cards.Count;
    }
}
