using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class CharacterSheet : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [SerializeField]Text _characterName = null;
        [SerializeField]Text _className = null;
        [SerializeField]Text _title = null;
        [SerializeField]Text _bio = null;
        [SerializeField]Text _level = null;
        [SerializeField]Text _experience = null;
        [SerializeField]Text _gold = null;
        [SerializeField]Text _stamina = null;
        [SerializeField]Text _endurance = null;
        [SerializeField]Text _attunement = null;
        [SerializeField]Text _resistance = null;
        [SerializeField]Text _strength = null;
        [SerializeField]Text _intellect = null;
        [SerializeField]Text _avoidance = null;
        [SerializeField]Text _precision = null;
        [SerializeField]Text _charisma = null;
        [SerializeField]Text _luck = null;
        [SerializeField]Text _armor = null;
        [SerializeField]Text _damageResistance = null;
        [SerializeField]Text _maxHealth = null;
        [SerializeField]Text _maxEnergy = null;
        [SerializeField]Text _maxMana = null;
        [SerializeField]Text _meleeDamage = null;
        [SerializeField]Text _meleeAttackSpeed = null;
        [SerializeField]Text _rangedDamage = null;
        [SerializeField]Text _rangedAttackSpeed = null;
        [SerializeField]Text _spellPower = null;
        [SerializeField]Text _castRate = null;
        [SerializeField]Text _maxWeight = null;
        bool _isOpen;
        Vector3 _offset;
        CanvasGroup _canvasGroup;

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update()
        {
            if (_isOpen)
            {
                _characterName.text = "Name: " + GameManager.character.characterInfo.name;
                _className.text = "Class: " + GameManager.character.characterClass.name;
                _title.text = "Title: " + GameManager.character.characterInfo.title;
                _bio.text = "Bio: " + GameManager.character.characterInfo.bio;
                _level.text = "Level: " + GameManager.character.characterInfo.level;
                _experience.text = "Experience: " + GameManager.character.characterInfo.experience;
                _gold.text = "Gold: " + GameManager.character.characterInfo.gold;
                _stamina.text = "Stamina: " + GameManager.character.stamina;
                _endurance.text = "Endurance: " + GameManager.character.endurance;
                _attunement.text = "Attunement: " + GameManager.character.attunement;
                _resistance.text = "Resistance: " + GameManager.character.resistance;
                _strength.text = "Strength: " + GameManager.character.strength;
                _intellect.text = "Intellect: " + GameManager.character.intellect;
                _avoidance.text = "Avoidance: " + GameManager.character.avoidance;
                _precision.text = "Precision: " + GameManager.character.precision;
                _charisma.text = "Charisma: " + GameManager.character.charisma;
                _luck.text = "Luck: " + GameManager.character.luck;
                _armor.text = "Armor: " + GameManager.character.armor;
                _damageResistance.text = "Damage Resistance: " + GameManager.character.damageResistance * 100 + "%";
                _maxHealth.text = "Maximum Health: " + GameManager.character.maxHealth;
                _maxEnergy.text = "Maximum Energy: " + GameManager.character.maxEnergy;
                _maxMana.text = "Maximum Mana: " + GameManager.character.maxMana;
                _meleeDamage.text = "Melee Damage: " + GameManager.character.meleeDamage;
                _meleeAttackSpeed.text = "Melee Attack Speed: " + GameManager.character.meleeAttackSpeed + "s";
                _rangedDamage.text = "Ranged Damage: " + GameManager.character.rangedDamage;
                _rangedAttackSpeed.text = "Ranged Attack Speed: " + GameManager.character.rangedAttackSpeed + "s";
                _spellPower.text = "Spell Power: " + GameManager.character.spellpower;
                _castRate.text = "Cast Rate: " + GameManager.character.castRate + "x";
                _maxWeight.text = "Weight Capacity: " + GameManager.character.maxWeight;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _offset = transform.position - Input.mousePosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition + _offset;
        }

        public void Toggle()
        {
            if (_canvasGroup.alpha == 1)
            {
                _isOpen = false;

                _canvasGroup.alpha = 0;
                _canvasGroup.blocksRaycasts = false;
            }
            else
            {
                _isOpen = true;

                _canvasGroup.alpha = 1;
                _canvasGroup.blocksRaycasts = true;
            }
        }
    }
}