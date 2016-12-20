using UnityEngine;
using RPG;

public class MinorArmorPotion : Item
{
    public MinorArmorPotion()
    {
        name = "Minor Armor Potion";
        icon = Resources.Load<Sprite>("Icons/potionRed");
        modelPrefab = Resources.Load<GameObject>("Items/MinorHealingPotion");
        stackSize = 5;
    }

    public override bool Use(Character character, Inventory inventory)
    {
        character.AddEffect(new MageArmorEffect());

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