using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CompoundEffect : Effect
{
    public EffectFragmentWrapper[] fragments;

    bool initialized = false;
    void Initialize()
    {
        Debug.Log("Initialized");
        foreach(EffectFragmentWrapper efw in fragments)
        {
            efw.GiveArguments();
        }
        initialized = true;
    }

    public override IEnumerator DoEffect()
    {
        if(!initialized)
            Initialize();

        foreach(EffectFragmentWrapper sf in fragments)
        {
            yield return sf.DoEffectFragment();
        }
    }

    public string GetDescription()
    {
        if(!initialized)
            Initialize();
        string to_return = "";

        foreach(EffectFragmentWrapper sf in fragments)
        {
            to_return += sf.GetDescription() + "\n";
        }

        return to_return;
    }
}
