﻿using UnityEngine;
using RPG;

public class RoundShield : Equipment
{
    public RoundShield()
    {
        name = "Round Shield";
        icon = Resources.Load<Sprite>("Icons/shield");
        modelPrefab = Resources.Load<GameObject>("Items/RoundShield");
        slot = Slot.OffHand;
        armor = 4;
    }

    public override bool Use(Character character, Inventory inventory)
    {
        if (character.equippedOffHand != null)
        {
            if (!character.equippedOffHand.Unequip(character, inventory))
            {
                return false;
            }
        }

        character.equippedOffHand = this;

        inventory.RemoveItem(this);	

        Show(character.offHandActiveMount);

        return true;
    }

    public override bool Unequip(Character character, Inventory inventory)
    {
        if (!inventory.AddItem(character.equippedOffHand))
        {
            return false;
        }

        character.equippedOffHand = null;

        MonoBehaviour.Destroy(worldModel);

        return true;
    }

    public override string Tooltip()
    {
        return 
			"<color=white><size=14><i>" + name + "</i></size></color>" + "\n" +
        "<color=white><size=14><i>" + "Armor: " + armor + "</i></size></color>";
    }
}