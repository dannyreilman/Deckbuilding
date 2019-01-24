using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName="Instant", fileName="New Instant")]
class InstantEffect : Buyable
{
    public string name;
    public Sprite sprite;
    public EffectFragmentWrapper[] fragments;
    public string tooltip;
    public string tooltipTitle;
    bool initialized = false;
    void Initialize()
    {
        foreach(EffectFragmentWrapper efw in fragments)
        {
            efw.GiveArguments();
        }
        initialized = true;
    }

    IEnumerator BuyCoroutine()
    {
        if(!initialized)
            Initialize();
        InputManager.instance.RegisterPlay();
        foreach(EffectFragmentWrapper sf in fragments)
        {
            yield return sf.DoEffect();
        }
        InputManager.instance.FinishPlay();
    }

    public override void Buy()
    {
        if(!initialized)
            Initialize();
        CardMonobehaviour.instance.StartCoroutine(BuyCoroutine());
    }

    public override string GetDescription()
    {
        if(!initialized)
            Initialize();
        string to_return = "(Do this effect immediately. This is not a card)\n\n";

        foreach(EffectFragmentWrapper sf in fragments)
        {
            to_return += sf.GetDescription() + "\n";
        }

        return to_return;
    }

    public override Sprite GetDisplay()
    {
        if(!initialized)
            Initialize();
        return sprite;
    }

    public override string GetName()
    {
        if(!initialized)
            Initialize();
        return name;
    }

    public override string GetTooltip()
    {
        if(!initialized)
            Initialize();
        return tooltip;
    }

    public override string GetTooltipTitle()
    {
        if(!initialized)
            Initialize();
        return tooltipTitle;
    }

    public override Color GetTypeColor()
    {
        if(!initialized)
            Initialize();
        return Color.gray;
    }

    public override string GetTypename()
    {
        if(!initialized)
            Initialize();
        return "Instant";
    }
}