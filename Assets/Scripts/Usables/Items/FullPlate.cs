using UnityEngine;
using RPG;

public class FullPlate : Equipment
{
    public FullPlate()
    {
        name = "Full Plate";
        icon = Resources.Load<Sprite>("Icons/upg_armor");
        modelPrefab = Resources.Load<GameObject>("Items/FullPlate");
        slot = Slot.Armor;
        armor = 8;
    }

    public override bool Use(Character character, Inventory inventory)
    {
        if (character.equippedArmor != null)
        {
            if (!character.equippedArmor.Unequip(character, inventory))
            {
                return false;
            }
        }

        character.equippedArmor = this;

        inventory.RemoveItem(this);	

        //Show (character.armorMount);

        return true;
    }

    public override bool Unequip(Character character, Inventory inventory)
    {
        if (!inventory.AddItem(character.equippedArmor))
        {
            return false;
        }

        character.equippedArmor = null;

        //MonoBehaviour.Destroy (WorldObject);

        return true;
    }

    public override string Tooltip()
    {
        return 
			"<color=white><size=14><i>" + name + "</i></size></color>" + "\n" +
        "<color=white><size=14><i>" + "Armor: " + armor + "</i></size></color>";
    }
}