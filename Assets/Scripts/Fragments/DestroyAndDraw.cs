using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName="Fragments/DestroyAndDraw", fileName="New Fragment")]
public class DestroyAndDraw : SelectFragment
{
    public override IEnumerator DoneSelecting(List<Card> selected)
    {
        foreach(Card c in selected)
        {
            c.MoveTo(GameplayManager.instance.destroyedCards);
            yield return GameplayManager.instance.DrawCard();   
        }
    }
}