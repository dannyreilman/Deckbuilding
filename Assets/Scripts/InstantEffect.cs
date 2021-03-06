using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName="Instant", fileName="New Instant")]
class InstantEffect : Buyable
{
    public Sprite sprite;
    public CompoundEffect buyEffect;

    public override IEnumerator Buy()
    {
        yield return InputManager.instance.PlayEffect(buyEffect);
    }

    protected override string GetGeneratedDescription()
    {
        string to_return = base.GetGeneratedDescription();
        to_return += "(Do this effect immediately. This is not a card)\n\n";
        foreach(EffectFragmentWrapper sf in buyEffect.fragments)
        {
            to_return += sf.GetDescription() + "\n";
        }

        return to_return;
    }
}