using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

[CreateAssetMenu(menuName="Enemies/BasicEnemy", fileName="Spell")]
public class Enemy : Buyable
{
    public int health;
    public int attack;

    public override IEnumerator Buy()
    {
        yield return new WaitForSeconds(0.1f);
        GameplayManager.instance.activeEnemies.Remove(this);

    }
    public void DealAttack()
    {
        GameplayManager.instance.health -= attack;
    }

    protected virtual void HandleClone(Enemy clone)
    {

    }

    public Enemy Clone()
    {
        Enemy e = (Enemy)this.MemberwiseClone();


        HandleClone(e);

        return e;
    }

    public bool SameCard(object other)
    {
        return base.Equals(other);
    }

    public override bool Equals(object other)
    {
        return ReferenceEquals(this, other);
    }

    public override int GetHashCode()
    {
        return RuntimeHelpers.GetHashCode(this);
    }
}
