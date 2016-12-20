using UnityEngine;
using RPG;

public class MinorEnergyPotion : Item
{
    public MinorEnergyPotion()
    {
        name = "Minor Energy Potion";
        icon = Resources.Load<Sprite>("Icons/potionGreen");
        modelPrefab = Resources.Load<GameObject>("Items/MinorHealingPotion");
        stackSize = 5;
    }

    public override bool Use(Character character, Inventory inventory)
    {
        if (character.currentEnergy >= character.maxEnergy)
        {
            return false;
        }

        character.RestoreEnergy(25);

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