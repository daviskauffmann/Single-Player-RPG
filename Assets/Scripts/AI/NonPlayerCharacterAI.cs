using System.Collections.Generic;
using UnityEngine;
using RPG;
using RPG.UI;

using Inventory = RPG.Inventory;
using Spellbook = RPG.Spellbook;

public class NonPlayerCharacterAI : MonoBehaviour
{
    [SerializeField]float _moveSpeed = 4.0f;
    [SerializeField]float _rotateSpeed = 5.0f;
    [SerializeField]float _aggroDistance = 10.0f;
    [SerializeField]float _followDistance = 5.0f;
    List<GameObject> _targets;
    GameObject _followTarget;
    Character _character;
    Animator _animator;
    Rigidbody _rigidbody;
    Inventory _inventory;
    Spellbook _spellbook;

    void Awake()
    {
        _character = GetComponent<Character>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _inventory = GetComponent<Inventory>();
        _spellbook = GetComponent<Spellbook>();
    }

    void FixedUpdate()
    {
        if (!_animator.GetBool("isDead"))
        {
            HandleMovement();
        }
    }

    void Update()
    {
        if (!_animator.GetBool("isDead"))
        {
            EquipItems();
            SetupAutoAttacks();
            HandleTargeting();
            HandleCombat();
            HandleOrders();
        }
    }

    void HandleMovement()
    {
        if (_animator.GetBool("isMoving"))
        {
            _rigidbody.MovePosition(transform.position + transform.forward * _moveSpeed * Time.deltaTime);
        }
    }

    void EquipItems()
    {
        for (int i = 0; i < _inventory.items.Count; i++)
        {
            if (_inventory.items[i].slot == Slot.Helmet && _character.equippedHelmet == null)
            {
                _inventory.items[i].Use(_character, _inventory);
                break;	
            }

            if (_inventory.items[i].slot == Slot.Armor && _character.equippedArmor == null)
            {
                _inventory.items[i].Use(_character, _inventory);
                break;
            }

            if (_inventory.items[i].slot == Slot.Cloak && _character.equippedCloak == null)
            {
                _inventory.items[i].Use(_character, _inventory);
                break;
            }

            if (_inventory.items[i].slot == Slot.MainHand && _character.equippedMainHand == null)
            {
                _inventory.items[i].Use(_character, _inventory);
                break;
            }

            if (_inventory.items[i].slot == Slot.OffHand && _character.equippedOffHand == null)
            {
                _inventory.items[i].Use(_character, _inventory);
                break;
            }

            if (_inventory.items[i].slot == Slot.Ranged && _character.equippedRanged == null)
            {
                _inventory.items[i].Use(_character, _inventory);
                break;
            }
        }
    }

    void SetupAutoAttacks()
    {
        for (int i = 0; i < _spellbook.spells.Count; i++)
        {
            if (_spellbook.spells[i].GetType() == typeof(AutoAttack))
            {
                _spellbook.meleeAutoAttack = _spellbook.spells[i];
                break;
            }
        }

        for (int i = 0; i < _spellbook.spells.Count; i++)
        {
            if (_spellbook.spells[i].GetType() == typeof(Shoot))
            {
                _spellbook.rangedAutoAttack = _spellbook.spells[i];
                break;
            }
        }
    }

    void HandleTargeting()
    {
        if (_character.selectedTarget != null)
        {
            if (_character.selectedTarget.GetComponent<Animator>().GetBool("isDead"))
            {
                _character.selectedTarget = null;
            }
        }
			
        _targets = new List<GameObject>();

        if (_character.hostileTags != null)
        {
            for (int i = 0; i < _character.hostileTags.Count; i++)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag(_character.hostileTags[i]);
                for (int j = 0; j < enemies.Length; j++)
                {
                    if (!enemies[j].GetComponent<Animator>().GetBool("isDead") && Vector3.Distance(enemies[j].transform.position, transform.position) < _aggroDistance)
                    {
                        _targets.Add(enemies[j]);
                    }
                }
            }
        }
			
        if (_targets.Count > 0)
        {
            _targets.Sort(delegate (GameObject x, GameObject y)
                {
                    return (Vector3.Distance(x.transform.position, transform.position).CompareTo(Vector3.Distance(y.transform.position, transform.position)));
                });

            _character.selectedTarget = _targets[0];
        }
			
        if (_character.selectedTarget != null)
        {
            _animator.SetBool("isInCombat", true);
        }
        else
        {
            _animator.SetBool("isInCombat", false);
            _animator.SetBool("isMoving", false);
        }
    }

    void HandleCombat()
    {
        if (_animator.GetBool("isInCombat"))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_character.selectedTarget.transform.position - transform.position), _rotateSpeed * Time.deltaTime);

            float distance = Vector3.Distance(_character.selectedTarget.transform.position, transform.position);

            if (_character.equippedRanged != null && _spellbook.rangedAutoAttack != null)
            {
                if (distance > _spellbook.rangedAutoAttack.maxRange)
                {
                    _animator.SetBool("isMoving", true);
                }
                else
                {
                    if (distance > _spellbook.rangedAutoAttack.minRange)
                    {
                        _animator.SetBool("isMoving", false);

                        _spellbook.CastSpell(_spellbook.rangedAutoAttack);
                    }
                    else
                    {
                        if (distance > _spellbook.meleeAutoAttack.maxRange)
                        {
                            _animator.SetBool("isMoving", true);
                        }
                        else
                        {
                            _animator.SetBool("isMoving", false);

                            _spellbook.CastSpell(_spellbook.meleeAutoAttack);
                        }
                    }
                }
            }
            else if (_spellbook.meleeAutoAttack != null)
            {
                if (distance > _spellbook.meleeAutoAttack.maxRange)
                {
                    _animator.SetBool("isMoving", true);
                }
                else
                {
                    _animator.SetBool("isMoving", false);

                    _spellbook.CastSpell(_spellbook.meleeAutoAttack);
                }
            }
        }
    }

    void HandleOrders()
    {
        if (this.tag == "Follower" && GameManager.character != null && GameManager.character.selectedTarget == this.gameObject)
        {
            if (Input.GetKeyUp(KeyCode.F1))
            {
                _followTarget = GameManager.character.gameObject;
            }

            if (Input.GetKeyUp(KeyCode.F2))
            {
                _followTarget = null;
            }
        }

        if (_followTarget != null && !_animator.GetBool("isInCombat"))
        {
            float distance = Vector3.Distance(_followTarget.transform.position, transform.position);

            if (distance > _followDistance)
            {
                _animator.SetBool("isMoving", true);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_followTarget.transform.position - transform.position), _rotateSpeed * Time.deltaTime);
            }

            if (distance <= _followDistance)
            {
                //transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (GameObject.Find ("PlayerCharacter").transform.position - transform.position), rotateSpeed * Time.deltaTime);
                _animator.SetBool("isMoving", false);
            }
        }
    }
}