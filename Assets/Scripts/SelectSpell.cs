using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SelectSpell: Spell
{
    public abstract int SelectHowMany();

    public abstract void DoneSelecting(List<Card> selected);

    public override void OnPlay()
    {
        base.OnPlay();
        InputManager.instance.Select(new Zone[]{GameplayManager.instance.hand}, SelectHowMany(), DoneSelecting);
    }
}