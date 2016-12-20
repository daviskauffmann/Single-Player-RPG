using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;
using RPG.UI;

using Random = UnityEngine.Random;
using Inventory = RPG.Inventory;
using Spellbook = RPG.Spellbook;
using Hotbar = RPG.Hotbar;

//TODO:
//	Refactor
//		Rename stuff
//		Encapsulate fields
//	Classes
//		Decides starting stats
//		Defines level curve and what base stats to increase and by how much
//		Defines starting spells
//		Weapon proficiencies?
//	Character sheet
//		Character info
//		Stats
//			Base stats
//			Equipment stats
//	Dialogue
//		Design window
//	Vendors
//		Design window
//	Professions
//		Alchemy - Potions
//			Potions restore stats
//		Inscription - Scrolls
//			Scrolls buff stats
//	Character creation
//	Multiple scenes
//		Persistence

//SAVING AND LOADING:
//  Game Manager
//      Dictionary<string, Transform>
//      Dictionary<string, Character>
//      Dictionary<string, Inventory>
//      Dictionary<string, Spellbook>
//      etc....
//      Potentially have all the values in serializable wrapper classes
//      (such as CharacterData or TransformData)
//      because MonoBehaviours are not serializable
//      This adds an extra step of parsing the data from the wrapper class, applying it to
//      a new instance of the component, and replacing the component on the GameObject
//      SAVING
//          foreach Dictionary<string, T>
//              foreach GameObject<UniqueId> in scene
//                  Add them to dictionary with UniqueId as key and T as value
//      LOADING
//          populate dictionaries from save file
//          OnSceneLoad
//              foreach Dictionary<string, T>
//                  foreach GameObject<UniqueId> in scene
//                      if UniqueId is in Dictionary<string, T>
//                          throw out T for that GameObject and replace with new T from save file
//                      if not, no problem, just let T keep its default values
//      When switching scenes, the Game Manager will update its dictionary of saveable
//      GameObjects with their values
//      When loading into a new scene, it will apply those values to any GameObjects that
//      exist in the dictionary
//      This ensures persistence between scenes
//      This simplifies the problem of saving and loading by making it only necessary
//      to save and load these dictionaries somehow
//      The Game Manager will automatically apply changes to GameObjects when loading a scene

public class Character : MonoBehaviour
{
    //public PlayerUI ui;

    public CharacterInfo characterInfo = new CharacterInfo();
    public CharacterClass characterClass = new CharacterClass();
    public StatMods statMods = new StatMods();

    public float currentHealth;
    public float currentEnergy;
    public float currentMana;

    public Equipment equippedHelmet;
    public Equipment equippedArmor;
    public Equipment equippedCloak;
    public Weapon equippedMainHand;
    public Equipment equippedOffHand;
    public Weapon equippedRanged;

    public GameObject helmetMount;
    public GameObject armorMount;
    public GameObject cloakMount;
    public GameObject mainHandActiveMount;
    public GameObject mainHandIdleMount;
    public GameObject offHandActiveMount;
    public GameObject offhandIdleMount;
    public GameObject rangedActiveMount;
    public GameObject rangedIdleMount;

    public EquipmentShown equipmentShown;

    public List<Effect> effects = new List<Effect>();
	
    public GameObject selectedTarget;
    public List<string> hostileTags;

    public bool cameraMoving;

    Animator _animator;
    TextMesh _nameplate;
    Inventory _inventory;
    Spellbook _spellbook;

    public int stamina
    {
        get { return characterClass.stamina + statMods.stamina; }
    }

    public int endurance
    {
        get { return characterClass.endurance + statMods.endurance; }
    }

    public int attunement
    {
        get { return characterClass.attunement + statMods.attunement; }
    }

    public int resistance
    {
        get { return characterClass.resistance + statMods.resistance; }
    }

    public int strength
    {
        get { return characterClass.strength + statMods.strength; }
    }

    public int intellect
    {
        get { return characterClass.intellect + statMods.intellect; }
    }

    public int avoidance
    {
        get { return Mathf.Clamp(characterClass.avoidance + statMods.avoidance, 1, 100); }
    }

    public int precision
    {
        get { return characterClass.precision + statMods.precision; }
    }

    public int charisma
    {
        get { return characterClass.charisma + statMods.charisma; }
    }

    public int luck
    {
        get { return characterClass.luck + statMods.luck; }
    }

    public float armor
    {
        get
        { 
            float equipmentArmor = 0;

            if (equippedHelmet != null)
            {
                equipmentArmor += equippedHelmet.armor;
            }

            if (equippedCloak != null)
            {
                equipmentArmor += equippedCloak.armor;
            }

            if (equippedArmor != null)
            {
                equipmentArmor += equippedArmor.armor;
            }

            if (equippedOffHand != null)
            {
                equipmentArmor += equippedOffHand.armor;
            }

            return statMods.armor + equipmentArmor;
        }
    }

    public float damageResistance
    {
        get { return Mathf.Clamp((resistance + (armor * 0.12f)) / 100, 0, 0.9f); }
    }

    public float maxHealth
    {
        get { return stamina * 100 + statMods.maxHealth; }
    }

    public float maxEnergy
    {
        get { return endurance * 100 + statMods.maxEnergy; }
    }

    public float maxMana
    {
        get { return attunement * 100 + statMods.maxMana; }
    }

    public float meleeDamage
    {
        get
        {
            float equipmentDamage = 0;
			
            if (equippedMainHand != null)
            {
                equipmentDamage += equippedMainHand.damage;
            }
			
            return statMods.meleeDamage + strength + endurance + luck + equipmentDamage;
        }
    }

    public float meleeAttackSpeed
    {
        get
        {
            float equipmentAttackSpeed = 1;

            if (equippedMainHand != null)
            {
                return equipmentAttackSpeed = equippedMainHand.attackSpeed;
            }

            return statMods.meleeAttackSpeed + equipmentAttackSpeed;
        }
    }

    public float rangedDamage
    {
        get
        {
            float equipmentDamage = 0;

            if (equippedRanged != null)
            {
                equipmentDamage += equippedRanged.damage;
            }

            return statMods.rangedDamage + strength / 2f + intellect / 2f + endurance / 2f + attunement / 2f + luck + equipmentDamage;
        }
    }

    public float rangedAttackSpeed
    {
        get
        {
            float equipmentAttackSpeed = 1;

            if (equippedRanged != null)
            {
                return equipmentAttackSpeed = equippedRanged.attackSpeed;
            }

            return statMods.rangedAttackSpeed + equipmentAttackSpeed;
        }	
    }

    public float spellpower
    {
        get { return statMods.spellPower + intellect + attunement + luck; }
    }

    public float castRate
    {
        get { return statMods.castRate + 1; }
    }

    public float maxWeight
    {
        get { return statMods.maxWeight + stamina * 100 + endurance * 100 + strength * 100; }
    }

    public float currentWeight
    {
        get
        { 
            float currentWeight = 0;

            for (int i = 0; i < _inventory.items.Count; i++)
            {
                currentWeight += _inventory.items[i].weight;
            }

            return currentWeight;
        }
    }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _nameplate = GetComponentInChildren<TextMesh>();
        _inventory = GetComponent<Inventory>();
        _spellbook = GetComponent<Spellbook>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        currentMana = maxMana;
    }

    void Update()
    {
        HandleDeath();
        UpdateNameplate();
        ShowEquipment();
        UpdateActiveEffects();
    }

    public void Move()
    {

    }

    public void RestoreHealth(float value)
    {
        if (value < 0)
        {
            return;
        }

        currentHealth += value;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void DamageHealth(float value)
    {
        if (value < 0)
        {
            return;
        }
		
        if (Random.Range(0, 100) < avoidance)
        {
            GetComponent<Animator>().SetTrigger("dodge");
        }
        else
        {
            value *= 1 - damageResistance;

            if (value < 1)
            {
                value = 1;
            }

            currentHealth -= value;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (_spellbook.prevSpell != null)
            {
                if (_spellbook.prevSpell.slowedByDamage)
                {
                    _spellbook.castTimer -= 0.25f;
                }
            }
        }
    }

    public void RestoreEnergy(float value)
    {
        if (value < 0)
        {
            return;
        }

        currentEnergy += value;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    public void DamageEnergy(float value)
    {
        if (value < 0)
        {
            return;
        }

        currentEnergy -= value;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    public void RestoreMana(float value)
    {
        if (value < 0)
        {
            return;
        }

        currentMana += value;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
    }

    public void DamageMana(float value)
    {
        if (value < 0)
        {
            return;
        }

        currentMana -= value;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
    }

    public void AddEffect(Effect effect)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].GetType() == effect.GetType())
            {
                effects[i].End(this);
            }
        }
			
        effect.Start(this);

        if (this == GameManager.character)
        {
            PlayerUI.auras.AddEffect(effect);
        }
        else
        {
            effects.Add(effect);
        }
    }

    public void RemoveEffect(Effect effect)
    {
        if (this == GameManager.character)
        {
            PlayerUI.auras.RemoveEffect(effect);
        }
        else
        {
            effects.Remove(effect);
        }
    }

    void HandleDeath()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;

            _animator.SetBool("isDead", true);
            _animator.SetBool("isMoving", false);
            _animator.SetBool("isMovingBack", false);

            /*if (equippedHelmet != null) {
				AddItem (equippedHelmet);
			}

			if (equippedCloak != null) {
				AddItem (equippedCloak);
			}

			if (equippedArmor != null) {
				AddItem (equippedArmor);
			}

			if (equippedMainHand != null) {
				AddItem (equippedMainHand);
			}

			if (equippedOffHand != null) {
				AddItem (equippedOffHand);
			}

			if (equippedRanged != null) {
				AddItem (equippedRanged);
			}*/
        }
    }

    void UpdateNameplate()
    {
        if (!_animator.GetBool("isDead"))
        {
            switch (transform.tag)
            {
                case "Player":
                    _nameplate.color = Color.blue;
                    break;
                case "Enemy":
                    _nameplate.color = Color.red;
                    break;
                case "Neutral":
                    _nameplate.color = Color.yellow;
                    break;
                case "Ally":
                    _nameplate.color = Color.green;
                    break;
                case "Follower":
                    _nameplate.color = Color.cyan;
                    break;
                default:
                    _nameplate.color = Color.white;
                    break;
            }
        }
        else
        {
            _nameplate.color = Color.grey;
        }
			
        if (GameManager.character != null && GameManager.character.selectedTarget == this.gameObject)
        {
            if (characterInfo.title != string.Empty)
            {
                _nameplate.text = characterInfo.name + "\n" + "<" + characterInfo.title + ">";
            }
            else
            {
                _nameplate.text = characterInfo.name;
            }
            _nameplate.transform.LookAt(_nameplate.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
        else
        {
            _nameplate.text = "";
        }
    }

    void ShowEquipment()
    {
        switch (equipmentShown)
        {
            case EquipmentShown.None:
                if (equippedMainHand != null)
                {
                    equippedMainHand.worldModel.transform.SetParent(mainHandIdleMount.transform);
                    equippedMainHand.worldModel.transform.localPosition = Vector3.zero;
                    equippedMainHand.worldModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }

                if (equippedOffHand != null)
                {
                    equippedOffHand.worldModel.transform.SetParent(offhandIdleMount.transform);
                    equippedOffHand.worldModel.transform.localPosition = Vector3.zero;
                    equippedOffHand.worldModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }

                if (equippedRanged != null)
                {
                    equippedRanged.worldModel.transform.SetParent(rangedIdleMount.transform);
                    equippedRanged.worldModel.transform.localPosition = Vector3.zero;
                    equippedRanged.worldModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }

                break;

            case EquipmentShown.MainHand:
                if (equippedMainHand != null)
                {
                    equippedMainHand.worldModel.transform.SetParent(mainHandActiveMount.transform);
                    equippedMainHand.worldModel.transform.localPosition = Vector3.zero;
                    equippedMainHand.worldModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }

                if (equippedOffHand != null)
                {
                    equippedOffHand.worldModel.transform.SetParent(offHandActiveMount.transform);
                    equippedOffHand.worldModel.transform.localPosition = Vector3.zero;
                    equippedOffHand.worldModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }

                if (equippedRanged != null)
                {
                    equippedRanged.worldModel.transform.SetParent(rangedIdleMount.transform);
                    equippedRanged.worldModel.transform.localPosition = Vector3.zero;
                    equippedRanged.worldModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }

                break;
            case EquipmentShown.Ranged:
                if (equippedMainHand != null)
                {
                    equippedMainHand.worldModel.transform.SetParent(mainHandIdleMount.transform);
                    equippedMainHand.worldModel.transform.localPosition = Vector3.zero;
                    equippedMainHand.worldModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }

                if (equippedOffHand != null)
                {
                    equippedOffHand.worldModel.transform.SetParent(offhandIdleMount.transform);
                    equippedOffHand.worldModel.transform.localPosition = Vector3.zero;
                    equippedOffHand.worldModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }

                if (equippedRanged != null)
                {
                    equippedRanged.worldModel.transform.SetParent(rangedActiveMount.transform);
                    equippedRanged.worldModel.transform.localPosition = Vector3.zero;
                    equippedRanged.worldModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }

                break;
        }
    }

    void UpdateActiveEffects()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].Update(this);
        }
    }
}

[Serializable]
public class CharacterInfo
{
    public string name;
    public string title;
    public string bio;
    public int level;
    public int experience;
    public int gold;

    public CharacterInfo()
    {
        name = "";
        title = "";
        bio = "";
        level = 1;
        experience = 0;
        gold = 0;
    }
}

[Serializable]
public class CharacterClass
{
    public string name;
    public int stamina;
    public int endurance;
    public int attunement;
    public int resistance;
    public int strength;
    public int intellect;
    public int avoidance;
    public int precision;
    public int charisma;
    public int luck;

    public CharacterClass()
    {
        name = "";
        stamina = 1;
        endurance = 1;
        attunement = 1;
        resistance = 1;
        strength = 1;
        intellect = 1;
        avoidance = 1;
        precision = 1;
        charisma = 1;
        luck = 1;
    }
}

[Serializable]
public class StatMods
{
    public int stamina;
    public int endurance;
    public int attunement;
    public int resistance;
    public int strength;
    public int intellect;
    public int avoidance;
    public int precision;
    public int charisma;
    public int luck;
    public float armor;
    public float maxHealth;
    public float maxEnergy;
    public float maxMana;
    public float meleeDamage;
    public float meleeAttackSpeed;
    public float rangedDamage;
    public float rangedAttackSpeed;
    public float spellPower;
    public float castRate;
    public float maxWeight;

    public StatMods()
    {
        stamina = 0;
        endurance = 0;
        attunement = 0;
        resistance = 0;
        strength = 0;
        intellect = 0;
        avoidance = 0;
        precision = 0;
        charisma = 0;
        luck = 0;
        armor = 0;
        maxHealth = 0;
        maxEnergy = 0;
        maxMana = 0;
        meleeDamage = 0;
        meleeAttackSpeed = 0;
        rangedDamage = 0;
        rangedAttackSpeed = 0;
        spellPower = 0;
        castRate = 0;
        maxWeight = 0;
    }
}

public enum Slot
{
    None,
    Helmet,
    Armor,
    Cloak,
    MainHand,
    OffHand,
    Ranged
}

public enum EquipmentShown
{
    None,
    MainHand,
    Ranged
}