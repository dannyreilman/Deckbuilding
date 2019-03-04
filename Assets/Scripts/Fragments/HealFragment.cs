using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName="Fragments/HealFragment", fileName="New Fragment")]
public class HealFragment : EffectFragment
{
    int howMuch;
    public override IEnumerator DoEffectFragment()
    {
        Debug.Log("Heal");
        GameplayManager.instance.health += howMuch;
        yield return new WaitForSeconds(0.1f);
    }
    public override void AcceptArguments(object[] arguments) 
    {
        howMuch = (int)arguments[0];
    }

    public override string GetDescription()
    {
        return "Restore " + howMuch + " health.";
    }

    public override Type[] GetArgumentTypes()
    {
        Type[] to_return = new Type[1];
        to_return[0] = EffectFragment.Type.Integer;
        return to_return;
    }
    public override string[] GetArgumentNames()
    {
        string[] to_return = new string[1];
        to_return[0] = "How Much?";
        return to_return;
    }
}