using UnityEngine;

[CreateAssetMenu(menuName="Cards/Resource", fileName="Resource")]
public class Resource : Card
{
    public int attack;
    public int coin;
    public int hammers;
    public int science;

    public override void OnPlay()
    {
        base.OnPlay();
        GameplayManager.instance.attack += attack;
        GameplayManager.instance.coin += coin;
        GameplayManager.instance.hammers += hammers;
        GameplayManager.instance.science += science;
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