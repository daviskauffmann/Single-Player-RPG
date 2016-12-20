using UnityEngine;
using System.Collections;
using RPG;

public class HolyLight : Spell
{
    public HolyLight()
    {
        name = "Holy Light";
        icon = Resources.Load<Sprite>("Icons/wand");
        particlePrefab = Resources.Load<GameObject>("Effects/Light/Holy Blast");
        cost = 5;
        maxRange = 20;
        castTime = 3;
    }

    public override IEnumerator Cast(Character character, Spellbook spellbook)
    {
        GameObject selectedTarget = character.selectedTarget;

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

        if (character.GetComponent<Animator>().GetBool("isMoving") || character.GetComponent<Animator>().GetBool("isMovingBack"))
        {
            yield break;
        }

        if (selectedTarget == null)
        {
            yield break;
        }

        if (selectedTarget.GetComponent<Animator>().GetBool("isDead"))
        {
            yield break;
        }

        if (character.hostileTags.Contains(selectedTarget.tag))
        {
            yield break;
        }

        if (Vector3.Distance(selectedTarget.transform.position, character.transform.position) > maxRange)
        {
            yield break;
        }

        if (Vector3.Dot((selectedTarget.transform.position - character.transform.position).normalized, character.transform.forward) <= 0)
        { 
            yield break;
        } 

        character.GetComponent<Animator>().SetTrigger("attack");
        spellbook.prevSpell = this;
        spellbook.globalCooldown = globalCooldown;
        spellbook.castTimer = 0;
        inProgress = true;

        while (spellbook.castTimer < castTime)
        {
            yield return null;
        }
			
        worldParticle = MonoBehaviour.Instantiate<GameObject>(particlePrefab);
        worldParticle.transform.position = selectedTarget.transform.position;
        MonoBehaviour.Destroy(worldParticle, particlePrefab.GetComponent<ParticleSystem>().duration);
        selectedTarget.GetComponent<Character>().RestoreHealth(character.spellpower);
        spellbook.prevSpell = null;
        cooldownTimer = cooldown;
        inProgress = false;
    }
}