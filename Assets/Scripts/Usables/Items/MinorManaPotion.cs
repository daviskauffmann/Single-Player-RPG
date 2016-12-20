using UnityEngine;
using RPG;

public class MinorManaPotion : Item
{
    public MinorManaPotion()
    {
        name = "Minor Mana Potion";
        icon = Resources.Load<Sprite>("Icons/potionBlue");
        modelPrefab = Resources.Load<GameObject>("Items/MinorHealingPotion");
        stackSize = 5;
    }

    public override bool Use(Character character, Inventory inventory)
    {
        if (character.currentMana >= character.maxMana)
        {
            return false;
        }

        character.RestoreMana(25);

        if (stack > 1)
        {
            stack--;
        }
        else
        {
            inventory.RemoveItem(this);
        }

        return true;
    }
}