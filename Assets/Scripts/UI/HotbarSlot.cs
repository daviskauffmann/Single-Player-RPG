using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class HotbarSlot : MonoBehaviour, IDropHandler
    {
        public int slotIndex;

        RPG.Hotbar _hotbar;
        [SerializeField]HotbarUsable _hotbarUsablePrefab = null;
        HotbarUsable _hotbarUsable;
        [SerializeField]Text _hotkey = null;

        void Awake()
        {
            _hotbar = GameManager.character.GetComponent<RPG.Hotbar>();
        }

        void Start()
        {
            _hotbarUsable = Instantiate<HotbarUsable>(_hotbarUsablePrefab);
            _hotbarUsable.transform.SetParent(transform);
            _hotbarUsable.transform.localPosition = Vector3.zero;
            _hotbarUsable.gameObject.SetActive(false);
        }

        void Update()
        {
            _hotkey.text = (slotIndex + 1).ToString();

            if (_hotbar.usables[slotIndex] != null)
            {
                _hotbarUsable.usable = _hotbar.usables[slotIndex];
                _hotbarUsable.gameObject.SetActive(true);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                if (eventData.pointerDrag.GetComponent<InventoryItem>() != null)
                {
                    InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();

                    _hotbarUsable.usable = (Usable)droppedItem.item;

                    _hotbar.usables[slotIndex] = _hotbarUsable.usable;

                    droppedItem.transform.SetParent(droppedItem.startParent);
                    droppedItem.transform.localPosition = Vector3.zero;
                }

                if (eventData.pointerDrag.GetComponent<SpellbookSpell>() != null)
                {
                    SpellbookSpell droppedItem = eventData.pointerDrag.GetComponent<SpellbookSpell>();

                    _hotbarUsable.usable = (Usable)droppedItem.spell;
                    _hotbar.usables[slotIndex] = _hotbarUsable.usable;

                    droppedItem.transform.SetParent(droppedItem.startParent);
                    droppedItem.transform.localPosition = Vector3.zero;
                }

                if (eventData.pointerDrag.GetComponent<HotbarUsable>() != null)
                {
                    HotbarUsable droppedItem = eventData.pointerDrag.GetComponent<HotbarUsable>();

                    _hotbarUsable.usable = droppedItem.usable;
                    _hotbar.usables[slotIndex] = _hotbarUsable.usable;
                }
            }
        }
    }
}