using UnityEngine;

namespace RPG.UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField]Inventory _inventoryPrefab = null;
        [SerializeField]Hotbar _hotbarPrefab = null;
        [SerializeField]CharacterSheet _characterSheetPrefab = null;
        [SerializeField]Spellbook _spellbookPrefab = null;
        [SerializeField]Loot _lootPrefab = null;
        [SerializeField]Storage _storagePrefab = null;
        [SerializeField]Auras _aurasPrefab = null;
        [SerializeField]Castbar _castBarPrefab = null;
        [SerializeField]StatusBars _statusBarsPrefab = null;
        [SerializeField]Tooltip _tooltipPrefab = null;

        public static PlayerUI instance { get; private set; }

        public static Inventory inventory { get; private set; }

        public static Hotbar hotbar { get; private set; }

        public static CharacterSheet characterSheet { get; private set; }

        public static Spellbook spellbook { get; private set; }

        public static Loot loot { get; private set; }

        public static Storage storage { get; private set; }

        public static Auras auras { get; private set; }

        public static Castbar castbar { get; private set; }

        public static StatusBars statusBars { get; private set; }

        public static Tooltip tooltip { get; private set; }

        void Awake()
        {
            instance = this;

            Activate();
        }

        void Update()
        {
            if (Input.GetButtonDown("Inventory"))
            {
                ToggleInventory();
            }

            if (Input.GetButtonDown("CharacterSheet"))
            {
                ToggleCharacterSheet();
            }

            if (Input.GetButtonDown("Spellbook"))
            {
                ToggleSpellbook();
            }
        }

        public void Activate()
        {
            inventory = Instantiate<Inventory>(_inventoryPrefab);
            inventory.transform.SetParent(transform, false);
            inventory.GetComponent<CanvasGroup>().alpha = 0;
            inventory.GetComponent<CanvasGroup>().blocksRaycasts = false;

            hotbar = Instantiate<Hotbar>(_hotbarPrefab);
            hotbar.transform.SetParent(transform, false);

            characterSheet = Instantiate<CharacterSheet>(_characterSheetPrefab);
            characterSheet.transform.SetParent(transform, false);
            characterSheet.GetComponent<CanvasGroup>().alpha = 0;
            characterSheet.GetComponent<CanvasGroup>().blocksRaycasts = false;

            spellbook = Instantiate<Spellbook>(_spellbookPrefab);
            spellbook.transform.SetParent(transform, false);
            spellbook.GetComponent<CanvasGroup>().alpha = 0;
            spellbook.GetComponent<CanvasGroup>().blocksRaycasts = false;

            loot = Instantiate<Loot>(_lootPrefab);
            loot.transform.SetParent(transform, false);
            loot.GetComponent<CanvasGroup>().alpha = 0;
            loot.GetComponent<CanvasGroup>().blocksRaycasts = false;

            storage = Instantiate<Storage>(_storagePrefab);
            storage.transform.SetParent(transform, false);
            storage.GetComponent<CanvasGroup>().alpha = 0;
            storage.GetComponent<CanvasGroup>().blocksRaycasts = false;

            auras = Instantiate<Auras>(_aurasPrefab);
            auras.transform.SetParent(transform, false);

            castbar = Instantiate<Castbar>(_castBarPrefab);
            castbar.transform.SetParent(transform, false);

            statusBars = Instantiate<StatusBars>(_statusBarsPrefab);
            statusBars.transform.SetParent(transform, false);

            tooltip = Instantiate<Tooltip>(_tooltipPrefab);
            tooltip.transform.SetParent(transform, false);
        }

        public void Deactivate()
        {
            instance = null;
            Destroy(inventory.gameObject);
            Destroy(hotbar.gameObject);
            Destroy(characterSheet.gameObject);
            Destroy(spellbook.gameObject);
            Destroy(loot.gameObject);
            Destroy(storage.gameObject);
            Destroy(auras.gameObject);
            Destroy(castbar.gameObject);
            Destroy(statusBars.gameObject);
            Destroy(tooltip.gameObject);
        }

        public void ToggleInventory()
        {
            inventory.Toggle();
        }

        public void ToggleCharacterSheet()
        {
            characterSheet.Toggle();
        }

        public void ToggleSpellbook()
        {
            spellbook.Toggle();
        }
    }
}