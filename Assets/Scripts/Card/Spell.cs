using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public string type;
    public override string GetTypename()
    {
        return ((type =="")?"":(type + " ")) + "Spell";
    }
    public override Color GetTypeColor()
    {
        return new Color(255, 0, 255);
    }

    public override IEnumerator OnPlay()
    {
        yield return base.OnPlay();
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
    public override string GetDescription()
    {
        string to_return = "";
        if(cards != 0)
        {
            to_return += "Draw " + cards.ToString() + " card" + (cards == 1?"":"s") + ".\n";
        }
        if(energy != 0)
        {
            to_return += "Gain " + energy.ToString() + " energy.\n";
        }
        if(attack != 0)
        {
            to_return += "Gain " + attack.ToString() + " attack.\n";
        }
        if(coin != 0)
        {
            to_return += "Gain " + coin.ToString() + " coin.\n";
        }
        if(hammers != 0)
        {
            to_return += "Gain " + hammers.ToString() + " hammer.\n";
        }
        if(science != 0)
        {
            to_return += "Gain " + science.ToString() + " science.";
        }

        return to_return;
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
