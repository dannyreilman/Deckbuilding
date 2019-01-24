using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName="Cards/FragmentedSpell", fileName="New Spell")]
public class FragmentedSpell : Spell
{
    public EffectFragmentWrapper[] fragments;
    bool initialized = false;
    void Initialize()
    {
        foreach(EffectFragmentWrapper efw in fragments)
        {
            efw.GiveArguments();
        }
        initialized = true;
    }

    protected override IEnumerator OnPlay()
    {
        if(!initialized)
            Initialize();
        yield return base.OnPlay();
        foreach(EffectFragmentWrapper sf in fragments)
        {
            yield return sf.DoEffect();
        }
    }

    public override string GetDescription()
    {
        if(!initialized)
            Initialize();
        string to_return = "";
        to_return += base.GetDescription();

        foreach(EffectFragmentWrapper sf in fragments)
        {
            to_return += sf.GetDescription() + "\n";
        }

        return to_return;
    }
}
