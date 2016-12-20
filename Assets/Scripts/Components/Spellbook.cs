using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.UI;

namespace RPG
{
    public class Spellbook : MonoBehaviour
    {
        public List<Spell> spells = new List<Spell>();
        public Coroutine prevCast;
        public Spell prevSpell;
        public float globalCooldown;
        public float castTimer;
        public bool targetingSpell;
        public Vector3 targetSpellPoint;
        public Spell meleeAutoAttack;
        public Spell rangedAutoAttack;

        Character _character;
        [SerializeField]string[] _spawnSpells = null;

        void Awake()
        {
            _character = GetComponent<Character>();
        }

        void Start()
        {
            SpawnSpells();
        }

        void Update()
        {
            GlobalCooldown();
            CastTimers();

            for (int i = 0; i < spells.Count; i++)
            {
                spells[i].Update();
            }
        }

        void SpawnSpells()
        {
/*            for (int i = 0; i < GameManager.instance.data.scenes.Count; i++)
            {
                if (GameManager.instance.data.scenes[i].sceneId == SceneManager.GetActiveScene().buildIndex)
                {
                    if (!GameManager.instance.data.scenes[i].reload)
                    {
                        return;
                    }
                }
            }*/

            if (_spawnSpells != null)
            {
                for (int i = 0; i < _spawnSpells.Length; i++)
                {
                    spells.Add((Spell)Activator.CreateInstance(Type.GetType(_spawnSpells[i])));
                }
            }
        }

        public void CastSpell(Spell spell)
        {
            if (prevSpell == null)
            {
                prevCast = StartCoroutine(spell.Cast(_character, this));
            }
        }

        public void InterruptSpell()
        {
            if (prevSpell != null && !prevSpell.castableWhileMoving && !targetingSpell)
            {
                StopCoroutine(prevCast);
                globalCooldown = 0;
                castTimer = 0;
                prevSpell.inProgress = false;
                prevSpell = null;
                prevCast = null;
                targetingSpell = false;
            }
        }

        public void AddSpell(Spell spell)
        {
            if (_character == GameManager.character)
            {
                PlayerUI.spellbook.AddSpell(spell);
            }
            else
            {
                spells.Add(spell);
            }
        }

        public void RemoveSpell(Spell spell)
        {
            if (_character == GameManager.character)
            {
                PlayerUI.spellbook.RemoveSpell(spell);
            }
            else
            {
                spells.Remove(spell);
            }
        }

        void GlobalCooldown()
        {
            if (globalCooldown > 0)
            {
                globalCooldown -= Time.deltaTime;
            }

            if (globalCooldown <= 0)
            {
                globalCooldown = 0;
            }
        }

        void CastTimers()
        {
            if (prevSpell != null && !targetingSpell)
            {
                if (castTimer < prevSpell.castTime)
                {
                    castTimer += Time.deltaTime * _character.castRate;
                }

                if (castTimer >= prevSpell.castTime)
                {
                    castTimer = prevSpell.castTime;
                }

                if (castTimer < 0)
                {
                    castTimer = 0;
                }
            }
        }
    }
}