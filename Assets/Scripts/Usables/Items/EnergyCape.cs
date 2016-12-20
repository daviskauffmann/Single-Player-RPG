using UnityEngine;
using RPG;

public class EnergyCape : Equipment
{
    public EnergyCape()
    {
        name = "Energy Cape";
        icon = Resources.Load<Sprite>("Icons/x");
        modelPrefab = Resources.Load<GameObject>("Items/EnergyCape");
        slot = Slot.Cloak;
    }

    public override bool Use(Character character, Inventory inventory)
    {
        if (character.equippedCloak != null)
        {
            if (!character.equippedCloak.Unequip(character, inventory))
            {
                return false;
            }
        }

        character.equippedCloak = this;
        character.statMods.stamina += 5;

        inventory.RemoveItem(this);

        Show(character.cloakMount);

        return true;
    }

    public override bool Unequip(Character character, Inventory inventory)
    {
        if (!inventory.AddItem(character.equippedCloak))
        {
            return false;
        }

        character.equippedCloak = null;
        character.statMods.stamina -= 5;

        MonoBehaviour.Destroy(worldModel);

        return true;
    }

    public override string Tooltip()
    {
        return 
			"<color=white><size=14><i>" + name + "</i></size></color>" + "\n" +
        "<color=white><size=14><i>" + "\t+5 Stamina" + "</i></size></color>";
    }
}