using UnityEngine;
using RPG;

public class WizardHat : Equipment
{
    public WizardHat()
    {
        name = "Wizard Hat";
        icon = Resources.Load<Sprite>("Icons/helmet");
        modelPrefab = Resources.Load<GameObject>("Items/WizardHat");
        slot = Slot.Helmet;
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
        character.statMods.intellect += 10;

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
        character.statMods.intellect -= 10;

        MonoBehaviour.Destroy(worldModel);

        return true;
    }

    public override string Tooltip()
    {
        return 
			"<color=white><size=14><i>" + name + "</i></size></color>" + "\n" +
        "<color=white><size=14><i>" + "\t+10 Intellect" + "</i></size></color>";
    }
}