using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SelectFragment: EffectFragment
{
    public int howMany;
    public bool upTo;

    public abstract IEnumerator DoneSelecting(List<Card> selected);

    public virtual Zone[] GetValidZones()
    {
        return new Zone[]{GameplayManager.instance.hand};
    }
    void DoneWrapper(List<Card> selected)
    {
        CardMonobehaviour.instance.StartCoroutine(DoneSelecting(selected));
    }

    public override IEnumerator DoEffectFragment()
    {
        InputManager.instance.Select(GetValidZones(), howMany, DoneWrapper, upTo);
        yield break;
    }
}