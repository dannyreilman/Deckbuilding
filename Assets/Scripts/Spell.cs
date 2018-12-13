using UnityEngine;

[CreateAssetMenu(menuName="Cards/BaseSpell", fileName="Spell")]
public class Spell : Card
{
    public int coin = 0;
    public int energy = 0;

    public int attack = 0;

    public int cards = 0;
    public int hammers = 0;
    public int science = 0;
    public int energyCost = 0;

    public override void OnPlay()
    {
        base.OnPlay();
        GameplayManager gm = GameplayManager.instance;
        gm.energy -= energyCost;
        gm.energy += energy;
        gm.attack += attack;
        gm.hammers += hammers;
        gm.science += science;
        gm.coin += coin;
        for(int i = 0; i < cards; ++i)
        {
            gm.DrawCard();
        }
    }

    public override bool CanPlay()
    {
        GameplayManager gm = GameplayManager.instance;
        InputManager im = InputManager.instance;
        return base.CanPlay()
            && gm.currentPhase == GameplayManager.Phase.Spells
            && gm.energy >= energyCost
            && im.currentMode == InputManager.InputMode.Playing;
    }

    protected override void HandleClone(Card c)
    {
        Spell b = (Spell)(c);
        b.coin = coin;
        b.energy = energy;
        b.attack = attack;
        b.cards = cards;
        b.hammers = hammers;
        b.science = science;
        b.energyCost = energyCost;
    }
}
