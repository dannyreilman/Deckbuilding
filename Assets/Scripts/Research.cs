using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class Research: Buyable
{
    const int RESEARCH_COUNT = 5;
    public Researchable r;

    public void Init(Researchable toResearch)
    {
        r = toResearch;
        name = "Research " + toResearch.name;
    }

    protected override string GetGeneratedDescription()
    {
        return "Add " + r.name + " to your shop. Its text is\n\n" + r.description;
    }

    public override IEnumerator Buy()
    {
        if(r is Card)
        {
            Card card = (Card)r;
            GameplayManager.instance.coinsShop.AddPile(card.name, card.buyCost);
            for(int i = 0; i < RESEARCH_COUNT; ++i)
            {
                GameplayManager.instance.coinsShop.AddElement(card.Clone());
            }
        }
        else
        {
            Blueprint bp = (Blueprint)r;
            GameplayManager.instance.hammersShop.AddPile(bp.name, bp.buyCost);
            GameplayManager.instance.hammersShop.AddElement(bp.Clone());
        }
        yield break;
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
