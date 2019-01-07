using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SelectSpell: Spell
{
    public bool upTo;
    public abstract int SelectHowMany();

    public abstract void DoneSelecting(List<Card> selected);

    public abstract string AfterSelectText();

    public virtual Zone[] GetValidZones()
    {
        return new Zone[]{GameplayManager.instance.hand};
    }

    public override IEnumerator OnPlay()
    {
        base.OnPlay();
        InputManager.instance.Select(GetValidZones(), SelectHowMany(), DoneSelecting, upTo);
        yield break;
    }

    public override string GetDescription()
    {
        string to_return = "";
        to_return += base.GetDescription() + "\n";
        to_return += "Select " + (upTo?" up to ":"") + SelectHowMany() + " cards from";
        Zone[] zones = GetValidZones();
        for(int i = 0; i < zones.Length; ++i)
        {
            to_return += " your " + zones[i].GetName();
            if(i < zones.Length - 1)
            {
                to_return += ",";
            }
            else
            {
                to_return += ".\n";
            }
        }

        to_return += AfterSelectText();
        return to_return;
    }
}