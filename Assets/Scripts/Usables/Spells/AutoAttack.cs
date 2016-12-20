using UnityEngine;
using System.Collections;
using RPG;

public class AutoAttack : Spell
{
    public AutoAttack()
    {
        name = "Auto Attack";
        icon = Resources.Load<Sprite>("Icons/sword");
        maxRange = 2;
        castableWhileMoving = true;
        castTime = 0.5f;
        slowedByDamage = false;
        globalCooldown = 0;
    }

    public override IEnumerator Cast(Character character, Spellbook spellbook)
    {
        character.GetComponent<Animator>().SetBool("isInCombat", true);
        GameObject selectedTarget = character.selectedTarget;

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

        character.equipmentShown = EquipmentShown.MainHand;

        if (Vector3.Distance(selectedTarget.transform.position, character.transform.position) > maxRange)
        {
            yield break;
        }

        if (Vector3.Dot((selectedTarget.transform.position - character.transform.position).normalized, character.transform.forward) <= 0)
        { 
            yield break;
        }

        character.GetComponent<Animator>().SetTrigger("attack");
        //spellbook.prevSpell = this;
        //spellbook.globalCooldown = globalCooldown;
        //spellbook.castTimer = 0;
        inProgress = true;

        /*while (spellbook.castTimer < castTime) {
			yield return new WaitForEndOfFrame ();
		}*/

        yield return new WaitForSeconds(castTime);

        if (selectedTarget != null)
        {
            if (character.equippedMainHand != null)
            {
                character.equippedMainHand.Hit(character, selectedTarget.GetComponent<Character>());
            }
            else
            {
                selectedTarget.GetComponent<Character>().DamageHealth(character.meleeDamage);
            }
        }

        //spellbook.prevSpell = null;
        cooldownTimer = character.meleeAttackSpeed;
        inProgress = false;
    }
}