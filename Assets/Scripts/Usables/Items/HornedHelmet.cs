using UnityEngine;
using RPG;

public class HornedHelmet : Equipment
{
    public HornedHelmet()
    {
        name = "Horned Helmet";
        icon = Resources.Load<Sprite>("Icons/helmet");
        modelPrefab = Resources.Load<GameObject>("Items/HornedHelmet");
        slot = Slot.Helmet;
        armor = 2;
    }

    public override bool Use(Character character, Inventory inventory)
    {
        if (character.equippedHelmet != null)
        {
            if (!character.equippedHelmet.Unequip(character, inventory))
            {
                return false;
            }
        }

        character.equippedHelmet = this;

        inventory.RemoveItem(this);	

        Show(character.helmetMount);

        return true;
    }

    public override bool Unequip(Character character, Inventory inventory)
    {
        if (!inventory.AddItem(character.equippedHelmet))
        {
            return false;
        }

        character.equippedHelmet = null;

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