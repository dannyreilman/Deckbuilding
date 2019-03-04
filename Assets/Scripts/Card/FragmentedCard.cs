using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName="Cards/FragmentedCard", fileName="New Card")]
public class FragmentedCard : Card
{
    public CompoundEffect playEffect;

    protected override IEnumerator OnPlay()
    {
        yield return base.OnPlay();
        yield return InputManager.instance.PlayEffect(playEffect);
    }

    protected override string GetGeneratedDescription()
    {
        string to_return = base.GetGeneratedDescription();
        to_return += playEffect.GetDescription();

        return to_return;
    }
}
