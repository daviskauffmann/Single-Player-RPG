using UnityEngine;
using RPG;

public class Bow : Weapon
{
    public Bow()
    {
        name = "Bow";
        icon = Resources.Load<Sprite>("Icons/bow");
        modelPrefab = Resources.Load<GameObject>("Items/Staff");
        slot = Slot.Ranged;
        damage = 3;
        attackSpeed = 1.1f;
    }

    public override bool Use(Character character, Inventory inventory)
    {
        if (character.equippedRanged != null)
        {
            if (!character.equippedRanged.Unequip(character, inventory))
            {
                return false;
            }
        }

        character.equippedRanged = this;

        inventory.RemoveItem(this);

        Show(character.rangedActiveMount);

        return true;
    }

    public override bool Unequip(Character character, Inventory inventory)
    {
        if (!inventory.AddItem(character.equippedRanged))
        {
            return false;
        }

        character.equippedRanged = null;

        MonoBehaviour.Destroy(worldModel);

        return true;
    }

    public override void Hit(Character character, Character target)
    {
        target.DamageHealth(character.rangedDamage);
    }

    public override string Tooltip()
    {
        return 
			"<color=white><size=14><i>" + name + "</i></size></color>" + "\n" +
        "<color=white><size=14><i>" + "Damage: " + damage + "</i></size></color>" + "\n" +
        "<color=white><size=14><i>" + "Attack Speed: " + attackSpeed + "s" + "</i></size></color>";
    }
}