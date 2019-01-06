public class EnemyStore : Store
{
    public EnemyStore(string name_in, Resource.Type type_in) 
        : base(name_in, type_in, true)
    {
        
    }

    public override void AddElement(Buyable element)
    {
        GameplayManager.instance.activeEnemies.Add((Enemy)element);
        base.AddElement(element);
    }

    public override void Buy(int index)
    {
        if(RightPhase() && GameplayManager.instance.attack > 0)
        {
            ((Enemy)piles[index].elements[0]).DealAttack();
        }
        base.Buy(index);
    }
}