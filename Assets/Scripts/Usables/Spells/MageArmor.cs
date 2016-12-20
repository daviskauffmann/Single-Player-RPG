using UnityEngine;
using System.Collections;
using RPG;

public class MageArmor : Spell
{
    public MageArmor()
    {
        name = "Mage Armor";
        icon = Resources.Load<Sprite>("Icons/armor");
        particlePrefab = Resources.Load<GameObject>("Effects/Wind/Cyclone");
        cost = 5;
        castTime = 2;
        cooldown = 30;
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

        if (character.GetComponent<Animator>().GetBool("isMoving") || character.GetComponent<Animator>().GetBool("isMovingBack"))
        {
            yield break;
        }

        character.GetComponent<Animator>().SetTrigger("attack");
        spellbook.prevSpell = this;
        spellbook.globalCooldown = globalCooldown;
        spellbook.castTimer = 0;
        inProgress = true;
        worldParticle = MonoBehaviour.Instantiate<GameObject>(particlePrefab);
        worldParticle.transform.position = character.transform.position;
        MonoBehaviour.Destroy(worldParticle, particlePrefab.GetComponent<ParticleSystem>().duration);

        while (spellbook.castTimer < castTime)
        {
            yield return null;
        }

        character.AddEffect(new MageArmorEffect());
        spellbook.prevSpell = null;
        cooldownTimer = cooldown;
        inProgress = false;
    }
}