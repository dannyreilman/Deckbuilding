using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SelectFragment: EffectFragment
{
    int howMany;
    bool upTo;

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

    public override void AcceptArguments(object[] arguments) 
    {
        howMany = (int)arguments[0];
        upTo = (bool)arguments[1];
    }

    public override string GetDescription()
    {
        return "Select " + howMany + " cards.";
    }

    public override Type[] GetArgumentTypes()
    {
        Type[] to_return = new Type[2];
        to_return[0] = EffectFragment.Type.Integer;
        to_return[1] = EffectFragment.Type.Boolean;
        return to_return;
    }
    public override string[] GetArgumentNames()
    {
        string[] to_return = new string[2];
        to_return[0] = "How Many";
        to_return[1] = "Up To";
        return to_return;
    }
}