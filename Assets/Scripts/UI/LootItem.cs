using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class LootItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public RPG.Inventory contents;
        public Item item;
        public Transform startParent;

        void Update()
        {
            if (!contents.items.Contains(item))
            {
                Destroy(gameObject);
            }

            item.slotIndex = 0;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayerUI.tooltip.Activate((Usable)item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayerUI.tooltip.Deactivate();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
            {
                if (PlayerUI.inventory.AddItem(item))
                {
                    PlayerUI.loot.RemoveItem(item);
                }
            }
        }
    }
}