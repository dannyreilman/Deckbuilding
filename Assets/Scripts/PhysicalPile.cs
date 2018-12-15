using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A pile with associated physical cards
public class PhysicalPile : Zone {
    public List<Card> cards = new List<Card>();
    public int GetCount()
    {
        return cards.Count;
    }

    public string name;

    Transform physicalParent;

    public PhysicalPile(string name_in, Transform transformIn)
    {
        name = name_in;
        physicalParent = transformIn;
    }

    public virtual string GetName()
    {
        return name;
    }

    public void DropCard(Card c)
    {
        Debug.Log("Zone " + name + " dropped");
        cards.Remove(c);
    }

    public void AddCard(Card c)
    {
        Debug.Log("Zone " + name + " gained");
        cards.Add(c);
        if(c.p_card == null)
        {
            PhysicalCardFactory.CreateCard(c);
        }
        c.p_card.transform.SetParent(physicalParent);
        c.p_card.attachedDrag.onRelease = () => {
            c.p_card.transform.localPosition = Vector3.zero;
            };
    }

    void MoveCard(DragBehaviour behaviour)
    {
        PhysicalCard attachedCard = behaviour.GetComponent<PhysicalCard>();
        if(attachedCard != null)
        {
            attachedCard.card.MoveTo(this);
        }
    }
}
