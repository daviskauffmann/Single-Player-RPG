using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace RPG.UI
{
    public class Inventory : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        RPG.Inventory _inventory;
        [SerializeField]GameObject _panel = null;
        [SerializeField]InventorySlot _inventorySlotPrefab = null;
        List<InventorySlot> _slots = new List<InventorySlot>();
        [SerializeField]InventoryItem _inventoryItemPrefab = null;
        [SerializeField]Text _gold = null;
        [SerializeField]Text _weight = null;
        bool _isOpen;
        Vector3 _offset;
        CanvasGroup _canvasGroup;

        void Awake()
        {
            _inventory = GameManager.character.GetComponent<RPG.Inventory>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update()
        {
            _gold.text = GameManager.character.characterInfo.gold.ToString();
            _weight.text = GameManager.character.currentWeight + " / " + GameManager.character.maxWeight;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Item hp = new MinorHealingPotion();
                hp.stack = 2;
                AddItem(hp);

                Item en = new MinorEnergyPotion();
                en.stack = 2;
                AddItem(en);

                Item mp = new MinorManaPotion();
                mp.stack = 2;
                AddItem(mp);

                Item ap = new MinorArmorPotion();
                ap.stack = 2;
                AddItem(ap);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                AddItem(new Shortsword());
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

            for (int i = 0; i < _inventory.slotAmount; i++)
            {
                InventorySlot inventorySlot = Instantiate<InventorySlot>(_inventorySlotPrefab);
                inventorySlot.transform.SetParent(_panel.transform);
                inventorySlot.slotIndex = i;
                _slots.Add(inventorySlot);
            }
				
            for (int i = 0; i < _inventory.items.Count; i++)
            {
                if (_slots[_inventory.items[i].slotIndex].transform.childCount == 0)
                {
                    InventoryItem inventoryItem = Instantiate<InventoryItem>(_inventoryItemPrefab);
                    inventoryItem.transform.SetParent(_slots[_inventory.items[i].slotIndex].transform);
                    inventoryItem.transform.localPosition = Vector3.zero;
                    inventoryItem.GetComponent<Image>().sprite = _inventory.items[i].icon;
                    inventoryItem.item = _inventory.items[i];
                }
                else
                {
                    for (int j = 0; j < _slots.Count; j++)
                    {
                        if (_slots[j].transform.childCount == 0)
                        {
                            InventoryItem inventoryItem = Instantiate<InventoryItem>(_inventoryItemPrefab);
                            inventoryItem.transform.SetParent(_slots[j].transform);
                            inventoryItem.transform.localPosition = Vector3.zero;
                            inventoryItem.GetComponent<Image>().sprite = _inventory.items[i].icon;
                            inventoryItem.item = _inventory.items[i];
                            break;
                        }
                    }
                }
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

        public bool AddItem(Item addItem)
        {
            if (_inventory.items.Count >= _inventory.slotAmount)
            {
                return false;
            }

            if (addItem.stack > 0)
            {
                _inventory.items.Add(addItem);

                if (_isOpen)
                {
                    Open();
                }
            }

            return true;
        }

        public void RemoveItem(Item removeItem)
        {
            _inventory.items.Remove(removeItem);

            if (_isOpen)
            {
                Open();
            }
        }

        public void Toggle()
        {
            if (_isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }
}