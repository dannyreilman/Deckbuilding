using UnityEngine;

[CreateAssetMenu(menuName="Cards/Resource", fileName="Resource")]
public class Resource : Card
{
    public int attack;
    public int coin;
    public int hammers;
    public int science;
    public string type;
    public override string GetCardType()
    {
        return ((type =="")?"":(type + " ")) + "Resource";
    }
    
    public override Color GetTypeColor()
    {
        if(attack > coin
        && attack > hammers
        && attack > science)
        {
            return new Color(255, 0, 0);
        }

        if(coin > attack
        && coin > hammers
        && coin > science)
        {
            return new Color(255, 255, 0);
        }

        if(hammers > attack
        && hammers > coin
        && hammers > science)
        {
            return new Color(255, 170, 0);
        }

        if(science > attack
        && science > coin
        && science > hammers)
        {
            return new Color(0, 255, 255);
        }

        return new Color(0, 255, 0);
    }

    public override void OnPlay()
    {
        base.OnPlay();
        GameplayManager.instance.attack += attack;
        GameplayManager.instance.coin += coin;
        GameplayManager.instance.hammers += hammers;
        GameplayManager.instance.science += science;
    }

    public override string GetCardText()
    {
        string to_return = "";
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


    public override bool CanPlay()
    {
        return base.CanPlay()
            && GameplayManager.instance.currentPhase == GameplayManager.Phase.Resources;
    }
    protected override void HandleClone(Card c)
    {
        Resource b = (Resource)(c);
        b.coin = coin;
        b.attack = attack;
        b.hammers = hammers;
        b.science = science;
    }

    //Override for special resources that shouldn't be played
    public virtual bool PlayAll()
    {
        return true;
    }
}