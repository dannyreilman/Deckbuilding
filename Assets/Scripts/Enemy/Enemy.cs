using UnityEngine;
using System.Runtime.CompilerServices;

[CreateAssetMenu(menuName="Enemies/BasicEnemy", fileName="Spell")]
public class Enemy : Buyable
{
    public string tooltipTitle;
    public string tooltip;

    public override string GetTooltip()
    {
        return tooltip;
    }

    public override string GetTooltipTitle()
    {
        return tooltipTitle;
    }
    public string enemyName;
    public int health;
    public int attack;
    public override string GetName()
    {
        return enemyName;
    }
    public override string GetTypename()
    {
        return "Enemy";
    }
    public override Color GetTypeColor()
    {
        return new Color(1, 0, 0);
    }
    public Sprite image;
    public override Sprite GetDisplay()
    {
        return image;
    }

    public override void Buy()
    {
        GameplayManager.instance.activeEnemies.Remove(this);
    }
    public void DealAttack()
    {
        GameplayManager.instance.health -= attack;
    }

    public override string GetDescription()
    {
        return "";
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
