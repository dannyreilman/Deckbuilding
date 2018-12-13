using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableZone : MonoBehaviour, Zone {
    public void DropCard(Card c)
    {
        Debug.Log("Zone " + name + " dropped");
    }

    public void AddCard(Card c)
    {
        Debug.Log("Zone " + name + " gained");
        if(c.p_card != null)
        {
            c.p_card.attachedDrag.onRelease = null;
            c.p_card.attachedDrag.enabled = false;
        }
    }

    void MoveCard(DragBehaviour behaviour)
    {
        PhysicalCard attachedCard = behaviour.GetComponent<PhysicalCard>();
        if(attachedCard != null)
        {
            attachedCard.card.MoveTo(this);
        }
    }

	// Use this for initialization
	void Start ()
    {
		GetComponent<DragCatcher>().onDragTo += MoveCard;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
