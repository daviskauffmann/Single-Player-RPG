using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class StorageSlot : MonoBehaviour, IDropHandler
    {
        public int slotIndex;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
				
                if (eventData.pointerDrag.GetComponent<StorageItem>() != null)
                {
                    StorageItem droppedItem = eventData.pointerDrag.GetComponent<StorageItem>();
                    StorageItem childItem = GetComponentInChildren<StorageItem>();

                    if (GetComponentInChildren<StorageItem>() == null)
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

                            droppedItem.transform.SetParent(droppedItem.startParent, false);
                            droppedItem.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            droppedItem.transform.SetParent(gameObject.transform);
                            droppedItem.transform.localPosition = Vector3.zero;

                            childItem.transform.SetParent(droppedItem.startParent, false);
                            childItem.transform.localPosition = Vector3.zero;
                        }
                    }
                }

                if (eventData.pointerDrag.GetComponent<InventoryItem>() != null)
                {
                    InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
                    StorageItem childItem = GetComponentInChildren<StorageItem>();

                    if (GetComponentInChildren<StorageItem>() == null)
                    {
                        droppedItem.item.slotIndex = slotIndex;
                        if (PlayerUI.storage.AddItem(droppedItem.item))
                        {
                            PlayerUI.inventory.RemoveItem(droppedItem.item);
                        }

                        droppedItem.transform.SetParent(droppedItem.startParent, false);
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

                            droppedItem.transform.SetParent(droppedItem.startParent, false);
                            droppedItem.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            childItem.item.slotIndex = droppedItem.item.slotIndex;

                            droppedItem.item.slotIndex = slotIndex;

                            PlayerUI.storage.RemoveItem(childItem.item);
                            PlayerUI.inventory.RemoveItem(droppedItem.item);
                            PlayerUI.inventory.AddItem(childItem.item);
                            PlayerUI.storage.AddItem(droppedItem.item);

                            droppedItem.transform.SetParent(droppedItem.startParent, false);
                            droppedItem.transform.localPosition = Vector3.zero;
                        }
                    }
                }
            }
        }
    }
}