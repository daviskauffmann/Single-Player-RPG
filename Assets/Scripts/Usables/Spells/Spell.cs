using UnityEngine;
using System.Collections;
using RPG;

public class Spell : Usable
{
    public float cooldownTimer;
    public bool inProgress;

    public GameObject particlePrefab { get; protected set; }

    public GameObject worldParticle { get; protected set; }

    public float cost { get; protected set; }

    public float maxRange { get; protected set; }

    public float minRange { get; protected set; }

    public bool castableWhileMoving { get; protected set; }

    public float castTime { get; protected set; }

    public bool slowedByDamage { get; protected set; }

    public float globalCooldown { get; protected set; }

    public float cooldown { get; protected set; }

    public Spell()
    {
        particlePrefab = null;
        worldParticle = null;
        cost = 0;
        maxRange = 0;
        minRange = 0;
        castableWhileMoving = false;
        castTime = 0;
        slowedByDamage = true;
        globalCooldown = 1.5f;
        cooldown = 0;
        cooldownTimer = 0;
        inProgress = false;
    }

    public override void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                cooldownTimer = 0;
            }
        }
    }

    public virtual IEnumerator Cast(Character character, Spellbook spellbook)
    {
        yield return null;
    }
}