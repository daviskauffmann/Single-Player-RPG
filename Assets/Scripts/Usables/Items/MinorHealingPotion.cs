using UnityEngine;
using RPG;

public class MinorHealingPotion : Item
{
    public MinorHealingPotion()
    {
        name = "Minor Healing Potion";
        icon = Resources.Load<Sprite>("Icons/potionRed");
        modelPrefab = Resources.Load<GameObject>("Items/MinorHealingPotion");
        stackSize = 5;
    }

    public override bool Use(Character character, Inventory inventory)
    {
        if (character.currentHealth >= character.maxHealth)
        {
            return false;
        }

        character.RestoreHealth(25);

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