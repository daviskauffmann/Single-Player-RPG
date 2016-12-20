using UnityEngine;
using RPG;

public class Staff : Weapon
{
    public Staff()
    {
        name = "Staff";
        icon = Resources.Load<Sprite>("Icons/wand");
        modelPrefab = Resources.Load<GameObject>("Items/Staff");
        slot = Slot.MainHand;
        damage = 5;
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
        character.statMods.castRate += 0.5f;

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
        character.statMods.castRate -= 0.5f;

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
        "<color=white><size=14><i>" + "Attack Speed: " + attackSpeed + "s" + "</i></size></color>" + "\n" +
        "<color=white><size=14><i>" + "\t+0.5x Cast Rate" + "</i></size></color>";
    }
}