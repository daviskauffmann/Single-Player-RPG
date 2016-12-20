using UnityEngine;
using System.Collections;
using RPG;

public class Shoot : Spell
{
    public Shoot()
    {
        name = "Shoot";
        icon = Resources.Load<Sprite>("Icons/bow");
        maxRange = 20;
        minRange = 8;
        castTime = 0.5f;
        slowedByDamage = false;
        globalCooldown = 0;
    }

    public override IEnumerator Cast(Character character, Spellbook spellbook)
    {
        character.GetComponent<Animator>().SetBool("isInCombat", true);
        GameObject selectedTarget = character.selectedTarget;

        if (character.equippedRanged == null)
        {
            yield break;
        }

        if (inProgress)
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

        /*if (character.GetComponent<Animator> ().GetBool ("isMoving") || character.GetComponent<Animator> ().GetBool ("isMovingBack")) {
			yield break;
		}*/

        if (selectedTarget == null)
        {
            yield break;
        }

        if (selectedTarget.GetComponent<Animator>().GetBool("isDead"))
        {
            yield break;
        }

        if (!character.hostileTags.Contains(selectedTarget.tag))
        {
            yield break;
        }

        character.equipmentShown = EquipmentShown.Ranged;

        if (Vector3.Distance(selectedTarget.transform.position, character.transform.position) > maxRange)
        {
            yield break;
        }

        if (Vector3.Distance(selectedTarget.transform.position, character.transform.position) < minRange)
        {
            yield break;
        }

        if (Vector3.Dot((selectedTarget.transform.position - character.transform.position).normalized, character.transform.forward) <= 0)
        { 
            yield break;
        }

        character.GetComponent<Animator>().SetTrigger("attack");
        //spellbook.prevSpell = this;
        //spellbook.globalCooldown = GlobalCooldown;
        //spellbook.castTimer = 0;
        inProgress = true;

        /*while (spellbook.castTimer < CastTime) {
			yield return null;
		}*/

        yield return new WaitForSeconds(castTime);


        if (selectedTarget != null)
        {
            character.equippedRanged.Hit(character, selectedTarget.GetComponent<Character>());
            if (selectedTarget.GetComponent<Character>().selectedTarget == null)
            {
                selectedTarget.GetComponent<Character>().selectedTarget = character.gameObject;
            }
        }

        //spellbook.prevSpell = null;
        cooldownTimer = character.rangedAttackSpeed;
        inProgress = false;
    }
}