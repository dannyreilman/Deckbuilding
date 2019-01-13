using UnityEngine;
public abstract class Buyable : ScriptableObject
{
    public abstract void Buy();
    public abstract Sprite GetDisplay();
    public abstract string GetName();
    public abstract string GetDescription();
    public abstract string GetTypename();
    public abstract Color GetTypeColor();
    public abstract string GetTooltip();
    public abstract string GetTooltipTitle();
}