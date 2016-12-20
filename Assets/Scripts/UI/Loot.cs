using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class Loot : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        public RPG.Inventory contents;

        [SerializeField]GameObject _panel = null;
        [SerializeField]LootSlot _lootSlotPrefab = null;
        List<LootSlot> _slots = new List<LootSlot>();
        [SerializeField]LootItem _lootItemPrefab = null;
        bool _isOpen;
        Vector3 _offset;
        CanvasGroup _canvasGroup;

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update()
        {
            if (_slots.Count == 0)
            {
                Close();

                contents = null;
            }

            if (_isOpen && Vector3.Distance(contents.transform.position, GameManager.character.transform.position) > 2)
            {
                Close();

                contents = null;
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

        public void Open()
        {
            Close();

            for (int i = 0; i < contents.items.Count; i++)
            {
                LootSlot lootSlot = Instantiate<LootSlot>(_lootSlotPrefab);
                lootSlot.transform.SetParent(_panel.transform);
                lootSlot.item = contents.items[i];

                _slots.Add(lootSlot);

                LootItem lootItem = Instantiate<LootItem>(_lootItemPrefab);
                lootItem.transform.SetParent(lootSlot.slot.transform);
                lootItem.transform.localPosition = Vector3.zero;
                lootItem.GetComponent<Image>().sprite = contents.items[i].icon;
                lootItem.contents = contents;
                lootItem.item = contents.items[i];
            }

            _isOpen = true;

            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
        }

        public void Close()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                Destroy(_slots[i].gameObject);
            }

            _slots.Clear();

            _isOpen = false;

            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }

        public void RemoveItem(Item removeItem)
        {
            contents.items.Remove(removeItem);

            if (_isOpen)
            {
                Open();
            }
        }
    }
}