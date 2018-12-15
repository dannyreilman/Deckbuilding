using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SelectSpell: Spell
{
    public abstract int SelectHowMany();

    public abstract void DoneSelecting(List<Card> selected);

    public abstract string AfterSelectText();

    public virtual Zone[] GetValidZones()
    {
        return new Zone[]{GameplayManager.instance.hand};
    }

    public override void OnPlay()
    {
        base.OnPlay();
        InputManager.instance.Select(GetValidZones(), SelectHowMany(), DoneSelecting);
    }

    public override string GetCardText()
    {
        string to_return = "";
        to_return += base.GetCardText() + "\n";
        to_return += "Select " + SelectHowMany() + " cards from";
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