using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Cards/DestroySelectedFromHand", fileName="Spell")]
public class DestroySelect : SelectSpell
{
    public int howMany = 0;

    public override void DoneSelecting(List<Card> selected)
    {
        foreach(Card c in selected)
        {
            c.MoveTo(GameplayManager.instance.destroyedCards);
        }
    }

    public override int SelectHowMany()
    {
        return howMany;
    }
}
