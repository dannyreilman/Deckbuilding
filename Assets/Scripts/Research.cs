using UnityEngine;
using System.Runtime.CompilerServices;



public class Research: Buyable
{
    const int RESEARCH_COUNT = 5;
    public Buyable b;

    public void Init(Buyable toResearch)
    {
        b = toResearch;
        name = "Research " + toResearch.name;
    }

    protected override string GetGeneratedDescription()
    {
        return "Add " + b.name + " to your shop. Its text is\n\n" + b.description;
    }

    public override void Buy()
    {
        if(b is Card)
        {
            Card card = (Card)b;
            GameplayManager.instance.coinsShop.AddPile(card.name, card.baseCost);
            for(int i = 0; i < RESEARCH_COUNT; ++i)
            {
                GameplayManager.instance.coinsShop.AddElement(card.Clone());
            }
        }
        else
        {
            Blueprint bp = (Blueprint)b;
            GameplayManager.instance.hammersShop.AddPile(bp.name, bp.baseCost);
            GameplayManager.instance.hammersShop.AddElement(bp.Clone());
        }
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
