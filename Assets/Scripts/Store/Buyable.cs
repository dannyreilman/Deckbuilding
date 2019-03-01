using UnityEngine;
public abstract class Buyable : ScriptableObject
{
    protected virtual string GetGeneratedDescription()
    {
        return "";
    }

    public abstract void Buy();
    public Sprite image;
    public new string name;
    
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
}