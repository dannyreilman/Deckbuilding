using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;

public class Card : ScriptableObject, Buyable
{

    public string tooltipTitle;
    public string tooltip;

    public string GetTooltip()
    {
        return tooltip;
    }

    public string GetTooltipTitle()
    {
        return tooltipTitle;
    }
    public string cardname;
    public int baseCost;
    public int researchCost;
    public string GetName()
    {
        return cardname;
    }
    public virtual string GetTypename()
    {
        return "";
    }
    public virtual Color GetTypeColor()
    {
        return new Color(255, 255, 255);
    }
    public Sprite image;
    public Sprite GetDisplay()
    {
        return image;
    }
    public void Buy()
    {
        MoveTo(GameplayManager.instance.discard);
    }

    [HideInInspector]
    public PhysicalCard p_card = null;
    Zone zone = null;

	public void PlayIfPlayable()
	{
		if(CanPlay())
		{
			CardMonobehaviour.instance.StartCoroutine(OnPlay());
		}
	}

    public Zone GetZone()
    {
        return zone;
    }

    public virtual string GetDescription()
    {
        return "";
    }

    public void MoveTo(Zone z)
    {
        if(ReferenceEquals(z, zone))
        {
            Debug.Log("Moved to same");
        }
        else
        {
            if(zone != null && !(zone.Equals(null)))
                zone.DropCard(this);
            z.AddCard(this);
            zone = z;
        }
    }

    public virtual IEnumerator OnPlay()
    {
        InputManager.instance.RegisterPlay();
        MoveTo(GameplayManager.instance.play);
        yield break;
    }

    public virtual bool CanPlay()
    {
        return InputManager.instance.currentMode == InputManager.InputMode.Playing
            && InputManager.instance.validZones.Contains(zone);
    }

    protected virtual void HandleClone(Card clone)
    {

    }

    public Card Clone()
    {
        Card c = (Card)this.MemberwiseClone();

        if(p_card != null)
        {
            c.p_card = null;
            PhysicalCardFactory.CreateCard(c);
        }

        HandleClone(c);

        return c;
    }

    public bool SameCard(object other)
    {
        return base.Equals(other);
    }

    public override bool Equals(object other)
    {
        return ReferenceEquals(this, other);
    }

    public override int GetHashCode()
    {
        return RuntimeHelpers.GetHashCode(this);
    }
}
