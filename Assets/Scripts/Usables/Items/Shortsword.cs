using UnityEngine;
using RPG;

public class Shortsword : Weapon
{
    public Shortsword()
    {
        name = "Shortsword";
        icon = Resources.Load<Sprite>("Icons/sword");
        modelPrefab = Resources.Load<GameObject>("Items/Shortsword");
        slot = Slot.MainHand;
        damage = 3;
        attackSpeed = 1.1f;
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