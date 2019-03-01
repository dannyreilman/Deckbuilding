using UnityEngine;
using System.Runtime.CompilerServices;

[CreateAssetMenu(menuName="Blueprints/Blueprint", fileName="Blueprint")]
public class Blueprint : Buyable
{
    public string blueprintName;
    public int baseCost;
    public int researchCost;

    public override void Buy()
    {
        GameplayManager.instance.built.Add(this);
    }

    public void OnStartOfTurn()
    {
    }

    public void OnActivate()
    {

    }

    //Override true if some activation is defined
    public bool Activateable()
    {
        return false;
    }
    

    protected virtual void HandleClone(Blueprint clone)
    {
    }

    public Blueprint Clone()
    {
        Blueprint b = (Blueprint)this.MemberwiseClone();

        HandleClone(b);

        return b;
    }

    public bool SameBlueprint(object other)
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
