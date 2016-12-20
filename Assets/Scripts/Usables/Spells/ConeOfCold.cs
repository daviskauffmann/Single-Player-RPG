using UnityEngine;
using System.Collections;
using RPG;

public class ConeOfCold : Spell
{
    public ConeOfCold()
    {
        name = "Cone Of Cold";
        icon = Resources.Load<Sprite>("Icons/wand");
        particlePrefab = Resources.Load<GameObject>("Effects/Water/Spray");
        cost = 5;
        cooldown = 20;
    }

    public override IEnumerator Cast(Character character, Spellbook spellbook)
    {
        if (inProgress)
        {
            yield break;
        }

        if (character.currentMana < cost)
        {
            yield break;
        }

        if (spellbook.globalCooldown > 0)
        {
            yield break;
        }

        if (cooldownTimer > 0)
        {
            yield break;
        }

        spellbook.prevSpell = this;
        spellbook.globalCooldown = globalCooldown;
        spellbook.castTimer = 0;
        inProgress = true;

        while (spellbook.castTimer < castTime)
        {
            yield return null;
        }

        worldParticle = MonoBehaviour.Instantiate<GameObject>(particlePrefab);
        worldParticle.transform.position = character.transform.position;
        worldParticle.transform.rotation = character.transform.rotation;
        MonoBehaviour.Destroy(worldParticle, particlePrefab.GetComponent<ParticleSystem>().duration);
        spellbook.prevSpell = null;
        cooldownTimer = cooldown;
        inProgress = false;
    }
}