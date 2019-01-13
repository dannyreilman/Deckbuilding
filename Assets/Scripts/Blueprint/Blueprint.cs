using UnityEngine;
using System.Runtime.CompilerServices;

[CreateAssetMenu(menuName="Blueprints/Blueprint", fileName="Blueprint")]
public class Blueprint : Buyable
{

    public string tooltipTitle;
    public string tooltip;

    public override string GetTooltip()
    {
        return tooltip;
    }

    public override string GetTooltipTitle()
    {
        return tooltipTitle;
    }
    public string blueprintName;
    public int baseCost;
    public int researchCost;
    public override string GetName()
    {
        return blueprintName;
    }
    public override string GetTypename()
    {
        return "Blueprint";
    }
    public override Color GetTypeColor()
    {
        return new Color(1, 0.6f, 0);
    }
    public Sprite image;
    public override Sprite GetDisplay()
    {
        return image;
    }
    public override void Buy()
    {
        GameplayManager.instance.built.Add(this);
    }

    public override string GetDescription()
    {
        return "";
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
