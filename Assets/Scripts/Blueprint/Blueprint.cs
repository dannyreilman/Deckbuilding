using UnityEngine;
using System.Runtime.CompilerServices;

[CreateAssetMenu(menuName="Blueprints/Blueprint", fileName="Blueprint")]
public class Blueprint : ScriptableObject, Buyable
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
    public string blueprintName;
    public int baseCost;
    public string GetName()
    {
        return blueprintName;
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
        GameplayManager.instance.built.Add(this);
    }

    public virtual string GetDescription()
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
