using UnityEngine;
using System.Collections;

public class BaseCardEffect : Effect
{
    public const float PLAY_DELAY = 0.01f;
    Card c;
    public BaseCardEffect(Card c_in)
    {
        c = c_in;
    }

    public override IEnumerator DoEffect()
    {
        yield return new WaitForSeconds(PLAY_DELAY);
        InputManager.instance.RegisterPlay();
        c.MoveTo(GameplayManager.instance.play);
        GameplayManager gm = GameplayManager.instance;
        gm.energy -= c.energyCost;
        gm.energy += c.energy;
        gm.attack += c.attack;
        gm.hammers += c.hammers;
        gm.science += c.science;
        gm.coin += c.coin;
        for(int i = 0; i < c.cards; ++i)
        {
            yield return gm.DrawCard();
        }
    }
}