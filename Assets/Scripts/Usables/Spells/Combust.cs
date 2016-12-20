using UnityEngine;
using System.Collections;
using RPG;

public class Combust : Spell
{
    public Combust()
    {
        name = "Combust";
        icon = Resources.Load<Sprite>("Icons/wand");
        particlePrefab = Resources.Load<GameObject>("Effects/Fire/Big Bang");
        cost = 5;
        maxRange = 20f;
        castTime = 2f;
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
			
        if (spellbook.targetingSpell)
        {
            yield break;
        }

        spellbook.prevSpell = this;
        spellbook.targetingSpell = true;
        inProgress = true;

        while (spellbook.targetingSpell)
        {
            yield return null;
        }

        character.GetComponent<Animator>().SetTrigger("attack");
        spellbook.globalCooldown = globalCooldown;
        spellbook.castTimer = 0;

        while (spellbook.castTimer < castTime)
        {
            yield return null;
        }

        worldParticle = MonoBehaviour.Instantiate<GameObject>(particlePrefab);
        worldParticle.transform.position = spellbook.targetSpellPoint;
        MonoBehaviour.Destroy(worldParticle, particlePrefab.GetComponent<ParticleSystem>().duration);
        spellbook.prevSpell = null;
        cooldownTimer = cooldown;
        inProgress = false;
    }
}