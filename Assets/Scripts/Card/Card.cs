using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName="Cards/BasicCard", fileName="New Card")]
public class Card : Buyable
{
    public bool IsType(string type)
    {
        string[] types = type.Split(' ');
        foreach(string typeToCheck in types)
        {
            if(type == typeToCheck)
                return true;
        }
        return false;
    }
    public int baseCost;
    public int researchCost;
    public int coin = 0;
    public int energy = 0;

    public int attack = 0;

    public int cards = 0;
    public int hammers = 0;
    public int science = 0;
    public int energyCost = 0;
    
    public override void Buy()
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
			CardMonobehaviour.instance.StartCoroutine(OnPlayWrapper());
		}
	}

    public Zone GetZone()
    {
        return zone;
    }

    protected override string GetGeneratedDescription()
    {
        string to_return = base.GetGeneratedDescription();
        if(cards != 0)
        {
            to_return += "Draw " + cards.ToString() + " card" + (cards == 1?"":"s") + ".\n";
        }
        if(energy != 0)
        {
            to_return += "Gain " + energy.ToString() + " energy.\n";
        }
        if(attack != 0)
        {
            to_return += "Gain " + attack.ToString() + " attack.\n";
        }
        if(coin != 0)
        {
            to_return += "Gain " + coin.ToString() + " coin.\n";
        }
        if(hammers != 0)
        {
            to_return += "Gain " + hammers.ToString() + " hammer.\n";
        }
        if(science != 0)
        {
            to_return += "Gain " + science.ToString() + " science.";
        }

        return to_return;
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

    public IEnumerator OnPlayWrapper()
    {
        yield return OnPlay();
    }

    BaseCardEffect onPlayEffect = null;
    protected virtual IEnumerator OnPlay()
    {
        if(onPlayEffect == null)
            onPlayEffect = new BaseCardEffect(this);
        yield return InputManager.instance.PlayEffect(onPlayEffect);
    }

    public virtual bool CanPlay()
    {
        return InputManager.instance.currentMode == InputManager.InputMode.Playing
            && InputManager.instance.currentValidZones.Contains(zone)
            && GameplayManager.instance.energy >= energyCost;
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
