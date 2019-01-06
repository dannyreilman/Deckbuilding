using UnityEngine;
using System.Runtime.CompilerServices;



public class Research: Buyable
{
    const int RESEARCH_COUNT = 5;
    Buyable b = null;
    public Research(Card c_in)
    {
        b = c_in;
    }
    public Research(Blueprint b_in)
    {
        b = b_in;
    }

    public string GetTooltip()
    {
        return b.GetTooltip();
    }

    public string GetTooltipTitle()
    {
        return b.GetTooltipTitle();
    }
    public string GetName()
    {
        return "Research " + b.GetName();
    }
    public virtual string GetTypename()
    {
        return b.GetTypename();
    }
    public virtual Color GetTypeColor()
    {
        return b.GetTypeColor();
    }
    public Sprite GetDisplay()
    {
        return b.GetDisplay();
    }
    public void Buy()
    {
        if(b is Card)
        {
            Card card = (Card)b;
            GameplayManager.instance.coinsShop.AddPile(card.GetName(), card.baseCost);
            for(int i = 0; i < RESEARCH_COUNT; ++i)
            {
                GameplayManager.instance.coinsShop.AddElement(card.Clone());
            }
        }
        else
        {
            Blueprint bp = (Blueprint)b;
            GameplayManager.instance.hammersShop.AddPile(bp.GetName(), bp.baseCost);
            GameplayManager.instance.hammersShop.AddElement(bp.Clone());
        }
    }

    public virtual string GetDescription()
    {
        return "Add " + b.GetName() + " to your shop. Its text is\n\n" + b.GetDescription();
    }

    protected virtual void HandleClone(Research clone)
    {

    }

    public Research Clone()
    {
        Research rc = (Research)this.MemberwiseClone();

        HandleClone(rc);

        return rc;
    }

    public bool SameResearch(object other)
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
