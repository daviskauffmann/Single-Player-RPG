using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class Storage : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        public RPG.Inventory contents;

        [SerializeField]GameObject _panel = null;
        [SerializeField]StorageSlot _storageSlotPrefab = null;
        List<StorageSlot> _slots = new List<StorageSlot>();
        [SerializeField]StorageItem _storageItemPrefab = null;
        bool _isOpen;
        Vector3 _offset;
        CanvasGroup _canvasGroup;

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update()
        {
            if (_isOpen && Vector3.Distance(contents.transform.position, GameManager.character.transform.position) > 2)
            {
                Close();
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
            Clear();

            for (int i = 0; i < contents.slotAmount; i++)
            {
                StorageSlot storageSlot = Instantiate<StorageSlot>(_storageSlotPrefab);
                storageSlot.transform.SetParent(_panel.transform);
                storageSlot.slotIndex = i;
                _slots.Add(storageSlot);
            }

            for (int i = 0; i < contents.items.Count; i++)
            {
                if (_slots[contents.items[i].slotIndex].transform.childCount == 0)
                {
                    StorageItem storageItem = Instantiate<StorageItem>(_storageItemPrefab);
                    storageItem.transform.SetParent(_slots[contents.items[i].slotIndex].transform);
                    storageItem.transform.localPosition = Vector3.zero;
                    storageItem.GetComponent<Image>().sprite = contents.items[i].icon;
                    storageItem.contents = contents;
                    storageItem.item = contents.items[i];
                }
                else
                {
                    for (int j = 0; j < _slots.Count; j++)
                    {
                        if (_slots[j].transform.childCount == 0)
                        {
                            StorageItem storageItem = Instantiate<StorageItem>(_storageItemPrefab);
                            storageItem.transform.SetParent(_slots[j].transform);
                            storageItem.transform.localPosition = Vector3.zero;
                            storageItem.GetComponent<Image>().sprite = contents.items[i].icon;
                            storageItem.contents = contents;
                            storageItem.item = contents.items[i];
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
            Clear();

            contents = null;
        }

        public bool AddItem(Item addItem)
        {
            if (contents.items.Count >= contents.slotAmount)
            {
                return false;
            }

            if (addItem.stack > 0)
            {
                //contents.items.Add(addItem);
                contents.AddItem(addItem);

                if (_isOpen)
                {
                    Open();
                }
            }

            return true;
        }

        public void RemoveItem(Item removeItem)
        {
            //contents.items.Remove(removeItem);
            contents.RemoveItem(removeItem);

            if (_isOpen)
            {
                Open();
            }
        }

        void Clear()
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
    }
}