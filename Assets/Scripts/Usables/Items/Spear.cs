using UnityEngine;
using RPG;

public class Spear : Weapon
{
    public Spear()
    {
        name = "Spear";
        icon = Resources.Load<Sprite>("Icons/upg_spear");
        modelPrefab = Resources.Load<GameObject>("Items/Spear");
        slot = Slot.MainHand;
        damage = 8;
        attackSpeed = 1.5f;
    }

    public override bool Use(Character character, Inventory inventory)
    {
        if (character.equippedMainHand != null)
        {
            if (!character.equippedMainHand.Unequip(character, inventory))
            {
                return false;
            }
        }

        character.equippedMainHand = this;

        inventory.RemoveItem(this);

        Show(character.mainHandActiveMount);

        return true;
    }

    public override bool Unequip(Character character, Inventory inventory)
    {
        if (!inventory.AddItem(character.equippedMainHand))
        {
            return false;
        }

        character.equippedMainHand = null;

        MonoBehaviour.Destroy(worldModel);

        return true;
    }

    public override void Hit(Character character, Character target)
    {
        target.DamageHealth(character.meleeDamage);
    }

    public override string Tooltip()
    {
        return 
			"<color=white><size=14><i>" + name + "</i></size></color>" + "\n" +
        "<color=white><size=14><i>" + "Damage: " + damage + "</i></size></color>" + "\n" +
        "<color=white><size=14><i>" + "Attack Speed: " + attackSpeed + "s" + "</i></size></color>";
    }
}