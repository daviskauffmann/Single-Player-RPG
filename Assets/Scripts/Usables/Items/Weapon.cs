using UnityEngine;

public class Weapon : Item
{
    public float damage;
    public float attackSpeed;

    public Weapon()
    {
        damage = 0;
        attackSpeed = 0;
    }

    public virtual void Hit(Character character, Character target)
    {

    }
}