using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName="Cards/FragmentedSpell", fileName="New Spell")]
public class FragmentedSpell : Spell
{
    public SpellFragment[] fragments;
    protected override IEnumerator OnPlay()
    {
        yield return base.OnPlay();
        foreach(SpellFragment sf in fragments)
        {
            yield return sf.Play();
        }
    }

    public override string GetDescription()
    {
        string to_return = "";
        to_return += base.GetDescription();

        foreach(SpellFragment sf in fragments)
        {
            to_return += sf.description + "\n";
        }

        return to_return;
    }
}
