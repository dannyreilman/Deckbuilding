using UnityEngine;
public interface Buyable
{
    void Buy();
    Sprite GetDisplay();
    string GetName();
    string GetDescription();
    string GetTypename();
    Color GetTypeColor();
    string GetTooltip();
    string GetTooltipTitle();
}