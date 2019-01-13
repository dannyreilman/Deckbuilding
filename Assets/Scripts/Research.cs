using UnityEngine;
using System.Runtime.CompilerServices;



public class Research: Buyable
{
    const int RESEARCH_COUNT = 5;
    public Buyable b;

    public void Init(Buyable toResearch)
    {
        b = toResearch;
    }

    public override string GetTooltip()
    {
        return b.GetTooltip();
    }

    public override string GetTooltipTitle()
    {
        return b.GetTooltipTitle();
    }
    public override string GetName()
    {
        return "Research " + b.GetName();
    }
    public override string GetTypename()
    {
        return b.GetTypename();
    }
    public override Color GetTypeColor()
    {
        return b.GetTypeColor();
    }
    public override Sprite GetDisplay()
    {
        return b.GetDisplay();
    }
    public override void Buy()
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

    public override string GetDescription()
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
