using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleEffect : Effect
{
    private EffectFragment effect;

    public SimpleEffect(EffectFragment e, object[] arguments)
    {
        effect = e;
        e.AcceptArguments(arguments);
    }

    public override IEnumerator DoEffect()
    {
        yield return effect.DoEffectFragment();
    }

    public string GetDescription()
    {
        return effect.GetDescription() + "\n";
    }
}
