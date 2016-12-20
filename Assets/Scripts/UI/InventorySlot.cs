using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public int slotIndex;

        RPG.Inventory _inventory;

        void Awake()
        {
            _inventory = GameManager.character.GetComponent<RPG.Inventory>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                if (eventData.pointerDrag.GetComponent<InventoryItem>() != null)
                {
                    InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
                    InventoryItem childItem = GetComponentInChildren<InventoryItem>();

                    if (childItem == null)
                    {
                        droppedItem.transform.SetParent(gameObject.transform);
                        droppedItem.transform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        if (droppedItem.item.GetType() == childItem.item.GetType() && droppedItem.item.stackSize > 1)
                        {
                            childItem.item.stack += droppedItem.item.stack;
                            droppedItem.item.stack = 0;

                            if (childItem.item.stack > childItem.item.stackSize)
                            {
                                droppedItem.item.stack = childItem.item.stack - childItem.item.stackSize;
                                childItem.item.stack = childItem.item.stackSize;
                            }

                            droppedItem.transform.SetParent(droppedItem.startParent);
                            droppedItem.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            droppedItem.transform.SetParent(gameObject.transform);
                            droppedItem.transform.localPosition = Vector3.zero;

                            childItem.transform.SetParent(droppedItem.startParent);
                            childItem.transform.localPosition = Vector3.zero;
                        }
                    }
                }

                if (eventData.pointerDrag.GetComponent<EquipmentItem>() != null)
                {
                    EquipmentItem droppedItem = eventData.pointerDrag.GetComponent<EquipmentItem>();
                    InventoryItem childItem = GetComponentInChildren<InventoryItem>();

                    if (childItem == null)
                    {
                        droppedItem.item.slotIndex = slotIndex;
                        droppedItem.item.Unequip(GameManager.character, _inventory);
                    }
                    else if (childItem.item.slot == droppedItem.item.slot)
                    {
                        droppedItem.item.slotIndex = slotIndex;
                        childItem.item.Use(GameManager.character, _inventory);
                    }

                    droppedItem.transform.SetParent(droppedItem.startParent);
                    droppedItem.transform.localPosition = Vector3.zero;
                }

                if (eventData.pointerDrag.GetComponent<StorageItem>() != null)
                {
                    StorageItem droppedItem = eventData.pointerDrag.GetComponent<StorageItem>();
                    InventoryItem childItem = GetComponentInChildren<InventoryItem>();

                    if (childItem == null)
                    {
                        droppedItem.item.slotIndex = slotIndex;

                        if (PlayerUI.inventory.AddItem(droppedItem.item))
                        {
                            PlayerUI.storage.RemoveItem(droppedItem.item);
                        }

                        droppedItem.transform.SetParent(droppedItem.startParent);
                        droppedItem.transform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        if (droppedItem.item.GetType() == childItem.item.GetType() && droppedItem.item.stackSize > 1)
                        {
                            childItem.item.stack += droppedItem.item.stack;
                            droppedItem.item.stack = 0;

                            if (childItem.item.stack > childItem.item.stackSize)
                            {
                                droppedItem.item.stack = childItem.item.stack - childItem.item.stackSize;
                                childItem.item.stack = childItem.item.stackSize;
                            }

                            droppedItem.transform.SetParent(droppedItem.startParent);
                            droppedItem.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            childItem.item.slotIndex = droppedItem.item.slotIndex;
                            droppedItem.item.slotIndex = slotIndex;

                            PlayerUI.inventory.RemoveItem(childItem.item);
                            PlayerUI.storage.RemoveItem(droppedItem.item);
                            PlayerUI.storage.AddItem(childItem.item);
                            PlayerUI.inventory.AddItem(droppedItem.item);

                            droppedItem.transform.SetParent(droppedItem.startParent);
                            droppedItem.transform.localPosition = Vector3.zero;
                        }
                    }
                }
            }
        }
    }
}