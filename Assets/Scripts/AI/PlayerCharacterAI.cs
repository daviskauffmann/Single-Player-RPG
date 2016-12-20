using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RPG;
using RPG.UI;

using Inventory = RPG.Inventory;
using Spellbook = RPG.Spellbook;
using Hotbar = RPG.Hotbar;

public class PlayerCharacterAI : MonoBehaviour
{
    [SerializeField]float _moveSpeed = 5.0f;
    [SerializeField]float _aggroDistance = 20.0f;
    List<GameObject> _targets;
    Character _character;
    Animator _animator;
    Rigidbody _rigidbody;
    Inventory _inventory;
    Spellbook _spellbook;
    Hotbar _hotbar;


    void Awake()
    {
        _character = GetComponent<Character>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _inventory = GetComponent<Inventory>();
        _spellbook = GetComponent<Spellbook>();
        _hotbar = GetComponent<Hotbar>();

        GameManager.character = _character;
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
            HandleHotBar();
            SetupAutoAttacks();
            HandleCombat();
            HandleMouseInput();
            HandleTargeting();
            ToggleWeapons();

            if (Input.GetKeyDown(KeyCode.F12))
            {
                _inventory.AddItem(new Bow());
                _inventory.AddItem(new BronzeShield());
                _inventory.AddItem(new EnergyCape());
                _inventory.AddItem(new FullPlate());
                _inventory.AddItem(new HalfPlate());
                _inventory.AddItem(new HornedHelmet());
                _inventory.AddItem(new MinorHealingPotion());
                _inventory.AddItem(new MinorEnergyPotion());
                _inventory.AddItem(new MinorManaPotion());
                _inventory.AddItem(new MinorArmorPotion());
                _inventory.AddItem(new OrnateShield());
                _inventory.AddItem(new RoundShield());
                _inventory.AddItem(new Shortsword());
                _inventory.AddItem(new Spear());
                _inventory.AddItem(new Staff());
                _inventory.AddItem(new WizardHat());
                _spellbook.AddSpell(new AutoAttack());
                _spellbook.AddSpell(new Shoot());
                _spellbook.AddSpell(new Fireball());
                _spellbook.AddSpell(new Regeneration());
                _spellbook.AddSpell(new ConeOfCold());
                _spellbook.AddSpell(new HolyLight());
                _spellbook.AddSpell(new Combust());
                _spellbook.AddSpell(new MageArmor());
            }
        }
    }

    void ToggleWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !_animator.GetBool("isInCombat"))
        {
            switch (_character.equipmentShown)
            {
                case EquipmentShown.None:
                    if (_character.equippedMainHand != null || _character.equippedOffHand != null)
                    {
                        _character.equipmentShown = EquipmentShown.MainHand;
                    }
                    else if (_character.equippedRanged != null)
                    {
                        _character.equipmentShown = EquipmentShown.Ranged;
                    }
                    else
                    {
                        _character.equipmentShown = EquipmentShown.None;
                    }
                    break;
                case EquipmentShown.MainHand:
                    if (_character.equippedRanged != null)
                    {
                        _character.equipmentShown = EquipmentShown.Ranged;
                    }
                    else
                    {
                        _character.equipmentShown = EquipmentShown.None;
                    }
                    break;
                case EquipmentShown.Ranged:
                    _character.equipmentShown = EquipmentShown.None;
                    break;
            }
        }
    }

    void HandleMovement()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            _spellbook.InterruptSpell();
            _animator.SetBool("isMoving", true);
            //_animator.speed = Input.GetAxis ("Vertical");
            _rigidbody.MovePosition(transform.position + transform.forward * _moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            _spellbook.InterruptSpell();
            _animator.SetBool("isMovingBack", true);
            //_animator.speed = -Input.GetAxis ("Vertical");
            _rigidbody.MovePosition(transform.position + transform.forward * _moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
        }

        if (Input.GetAxis("Vertical") == 0)
        {
            _animator.SetBool("isMoving", false);
            _animator.SetBool("isMovingBack", false);
            //_animator.speed = 1;
        }
    }

    void HandleHotBar()
    {
        for (int i = 0; i < _hotbar.usables.Length; i++)
        {
            if (_hotbar.usables[i] != null)
            {
                if (Input.GetButtonDown("Hotbar " + (i + 1).ToString()))
                {
                    if (_hotbar.usables[i] is Item)
                    {
                        Item item = (Item)_hotbar.usables[i];
                        item.Use(_character, _inventory);
                    }

                    if (_hotbar.usables[i] is Spell)
                    {
                        Spell spell = (Spell)_hotbar.usables[i];
                        _spellbook.CastSpell(spell);
                    }
                }
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

    void HandleCombat()
    {
        if (_animator.GetBool("isInCombat") && _character.selectedTarget != null)
        {
            float distance = Vector3.Distance(_character.selectedTarget.transform.position, transform.position);

            if (_character.equippedRanged != null && _spellbook.rangedAutoAttack != null && _character.selectedTarget != null && distance > _spellbook.rangedAutoAttack.minRange)
            {
                _spellbook.CastSpell(_spellbook.rangedAutoAttack);
            }
            else if (_spellbook.meleeAutoAttack != null && _character.selectedTarget != null && distance > _spellbook.meleeAutoAttack.minRange)
            {
                _spellbook.CastSpell(_spellbook.meleeAutoAttack);
            }
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetButtonUp("Fire1") && !EventSystem.current.IsPointerOverGameObject() && !_character.cameraMoving)
        {
            LayerMask layerMask = 1 << 9;
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.GetComponent<Character>() != null)
                {
                    _character.selectedTarget = hit.transform.gameObject;
                }
            }
            else
            {
                _character.selectedTarget = null;
            }			
        }
			
        if (Input.GetButtonUp("Fire2") && !EventSystem.current.IsPointerOverGameObject() && !_character.cameraMoving)
        {
            LayerMask layerMask = 1 << 9;
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.GetComponent<Character>() != null)
                {
                    if (_character.hostileTags.Contains(hit.transform.tag))
                    {
                        _character.selectedTarget = hit.transform.gameObject;

                        _animator.SetBool("isInCombat", true);
                    }
                    else
                    {
                        _character.selectedTarget = hit.transform.gameObject;
                    }
                }
            }
        }

        if (Input.GetButtonDown("Fire2") && !EventSystem.current.IsPointerOverGameObject() && !_character.cameraMoving)
        {
            LayerMask layerMask = 1 << 9;
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.GetComponent<WorldObject>() != null)
                {
                    if (PlayerUI.inventory.AddItem(hit.transform.GetComponent<WorldObject>().item))
                    {
                        Destroy(hit.transform.gameObject);

                        GameManager.instance.data.RemoveWorldObjectData(hit.transform.GetComponent<WorldObject>().id);
                        GameManager.instance.data.RemoveTransformData(hit.transform.GetComponent<WorldObject>().id);
                    }
                }

                /*if (hit.transform.GetComponent<RPG.Inventory> () != null) {
					PlayerUI.loot.contents = hit.transform.GetComponent<RPG.Inventory> ();
					PlayerUI.loot.Open ();
				}*/

                if (hit.transform.GetComponent<RPG.Inventory>() != null)
                {
                    PlayerUI.storage.contents = hit.transform.GetComponent<RPG.Inventory>();
                    PlayerUI.storage.Open();
                }
            }
        }

        if (_spellbook.targetingSpell && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (Vector3.Distance(transform.position, hit.point) <= _spellbook.prevSpell.maxRange)
                {
                    //GameObject targetReticle = (GameObject)Instantiate (Resources.Load<GameObject> ("Effects/Fire/Fire_03"), hit.point, Quaternion.identity);
                    //Destroy (targetReticle, targetReticle.GetComponent<ParticleSystem> ().duration);
                }
                else
                {
                    //GameObject targetReticle = (GameObject)Instantiate (Resources.Load<GameObject> ("Effects/Fire/Fire_01"), hit.point, Quaternion.identity);
                    //Destroy (targetReticle, targetReticle.GetComponent<ParticleSystem> ().duration);
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (Vector3.Distance(transform.position, hit.point) <= _spellbook.prevSpell.maxRange)
                {
                    _spellbook.targetSpellPoint = hit.point;
                    _spellbook.targetingSpell = false;
                }
            }

            if (Input.GetButtonUp("Fire2") && !_character.cameraMoving)
            {
                _spellbook.targetingSpell = false;

                _spellbook.InterruptSpell();
            }
        }
    }

    void HandleTargeting()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _targets = new List<GameObject>();

            if (_character.hostileTags != null)
            {
                for (int i = 0; i < _character.hostileTags.Count; i++)
                {
                    GameObject[] enemy = GameObject.FindGameObjectsWithTag(_character.hostileTags[i]);

                    for (int j = 0; j < enemy.Length; j++)
                    {
                        if (!enemy[j].GetComponent<Animator>().GetBool("isDead") && Vector3.Distance(enemy[j].transform.position, transform.position) < _aggroDistance)
                        {
                            _targets.Add(enemy[j]);
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

                if (_character.selectedTarget == null)
                {
                    _character.selectedTarget = _targets[0];
                }
                else
                {
                    int index = _targets.IndexOf(_character.selectedTarget);

                    if (index < _targets.Count - 1)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }

                    _character.selectedTarget = _targets[index];
                }
            }
        }
			
        if (_character.selectedTarget == null || !_character.hostileTags.Contains(_character.selectedTarget.tag) | _character.selectedTarget.GetComponent<Animator>().GetBool("isDead"))
        {
            _animator.SetBool("isInCombat", false);
        }
    }
}