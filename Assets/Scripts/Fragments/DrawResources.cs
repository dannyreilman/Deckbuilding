using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName="Fragments/DrawResources", fileName="New Spell")]
public class DrawResources : EffectFragment
{
    public int howMany;
    private IEnumerator Draw(List<Card> cards)
    {
        for(int i = cards.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(0.1f);
            cards[i].MoveTo(GameplayManager.instance.hand);
        }
    }
    public override IEnumerator DoEffect()
    {
        List<Card> cardsToDraw = new List<Card>();
        for(int i = GameplayManager.instance.deck.cards.Count - 1; i >= 0; i--)
        {
            Card c = GameplayManager.instance.deck.cards[i];
            if(c is Resource)
            {
                cardsToDraw.Add(c);
                if(cardsToDraw.Count >= howMany)
                {
                    yield return Draw(cardsToDraw);
                    yield break;
                }
            }
        }
    
        for(int i = GameplayManager.instance.discard.cards.Count - 1; i >= 0; i--)
        {
            Card c = GameplayManager.instance.discard.cards[i];
            if(c is Resource)
            {
                cardsToDraw.Add(c);
                if(cardsToDraw.Count >= howMany)
                {
                    yield return Draw(cardsToDraw);
                    yield break;
                }
            }
        }
        yield return Draw(cardsToDraw);
    }
}