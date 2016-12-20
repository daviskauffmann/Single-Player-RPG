using UnityEngine;

public class MageArmorEffect : Effect
{
    public MageArmorEffect()
    {
        name = "Mage Armor";
        icon = Resources.Load<Sprite>("Icons/armor");
        duration = 20;
        durationTimer = 0;
    }

    public override void Start(Character character)
    {
        character.statMods.resistance += 25;
    }

    public override void End(Character character)
    {
        base.End(character);

        character.statMods.resistance -= 25;
    }
}