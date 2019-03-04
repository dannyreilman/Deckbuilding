using UnityEngine;
using System;
using System.Collections;
public abstract class Buyable : ScriptableObject
{
    protected virtual string GetGeneratedDescription()
    {
        return "";
    }

    public enum Rarity
    {
        None,
        Common,
        Rare,
        Epic,
        Mythic
    }
    public static float GetRarityWeight(Rarity r)
    {
        switch(r)
        {
            case Rarity.Common:
                return 0.5f;
            case Rarity.Rare:
                return 0.4f;
            case Rarity.Epic:
                return 0.09f;
            case Rarity.Mythic:
                return 0.01f;
            default:
                return 0.0f;
        }
    }

    public Rarity rarity;

    public abstract IEnumerator Buy();
    public Sprite image;
    
    [SerializeField]
    private string description_internal;
    private bool generatedDescription;

    public string description
    {
        get
        {
            if(!generatedDescription)
            { 
                description_internal = GetGeneratedDescription() + description_internal;
                generatedDescription = true;
            }

            return description_internal;
        }
    }

    [SerializeField]
    private string typeName_internal;
    public string typeName
    {
        get
        {
            return typeName_internal;
        }

        set
        {
            if(typeName_internal != value)
            {
                typeName_internal = value;
                typeColor_internal = TypeColorSelector.instance.DetermineTypeColor(typeName);
            }
        }
    }
    public bool IsType(string type)
    {
        string[] types = typeName.Split(' ');
        foreach(string typeToCheck in types)
        {
            if(type == typeToCheck)
                return true;
        }
        return false;
    }

    private Color typeColor_internal = Color.clear;
    [HideInInspector]
    public Color typeColor
    {
        get
        {
            if(typeColor_internal == Color.clear)
                typeColor_internal = TypeColorSelector.instance.DetermineTypeColor(typeName);
            return typeColor_internal;
        }
    }
    public string tooltip;
    public string tooltipTitle;

    public int buyCost;
}