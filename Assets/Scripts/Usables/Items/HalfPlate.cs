using UnityEngine;
using RPG;

public class HalfPlate : Equipment
{
    public HalfPlate()
    {
        name = "Half Plate";
        icon = Resources.Load<Sprite>("Icons/armor");
        modelPrefab = Resources.Load<GameObject>("Items/HalfPlate");
        slot = Slot.Armor;
        armor = 5;
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