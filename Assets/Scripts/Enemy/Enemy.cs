using UnityEngine;
using System.Runtime.CompilerServices;

[CreateAssetMenu(menuName="Enemies/BasicEnemy", fileName="Spell")]
public class Enemy : ScriptableObject, Buyable
{
    public string tooltipTitle;
    public string tooltip;

    public string GetTooltip()
    {
        return tooltip;
    }

    public string GetTooltipTitle()
    {
        return tooltipTitle;
    }
    public string enemyName;
    public int health;
    public int attack;
    public string GetName()
    {
        return enemyName;
    }
    public virtual string GetTypename()
    {
        return "Enemy";
    }
    public virtual Color GetTypeColor()
    {
        return new Color(1, 0, 0);
    }
    public Sprite image;
    public Sprite GetDisplay()
    {
        return image;
    }

    public void Buy()
    {
        GameplayManager.instance.activeEnemies.Remove(this);
    }
    public void DealAttack()
    {
        GameplayManager.instance.health -= attack;
    }

    public virtual string GetDescription()
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
