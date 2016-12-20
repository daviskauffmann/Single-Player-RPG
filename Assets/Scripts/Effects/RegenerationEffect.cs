using UnityEngine;

public class RegenerationEffect : Effect
{
    public RegenerationEffect()
    {
        name = "Regeneration";
        icon = Resources.Load<Sprite>("Icons/wand");
        duration = 5;
        durationTimer = 0;
    }

    public override void Update(Character character)
    {
        base.Update(character);

        character.RestoreHealth(Time.deltaTime * character.spellpower);
    }
}