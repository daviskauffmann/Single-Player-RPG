using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Item item;
        public Transform startParent;

        RPG.Inventory _inventory;
        [SerializeField]Text _stack = null;
        CanvasGroup _canvasGroup;

        void Awake()
        {
            _inventory = GameManager.character.GetComponent<RPG.Inventory>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update()
        {
            if (!_inventory.items.Contains(item))
            {
                Destroy(gameObject);
            }

            if (GetComponentInParent<InventorySlot>() != null)
            {
                item.slotIndex = GetComponentInParent<InventorySlot>().slotIndex;
            }

            if (item.stack > 1)
            {
                _stack.text = item.stack.ToString();
            }
            else
            {
                _stack.text = null;
            }

            if (item.stack < 1)
            {
                PlayerUI.inventory.RemoveItem(item);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            startParent = transform.parent;
            transform.SetParent(PlayerUI.instance.transform);

            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (transform.parent == PlayerUI.instance.transform)
            {	
                item.slotIndex = 0;
                item.Drop(_inventory);
                Destroy(gameObject);
            }

            _canvasGroup.blocksRaycasts = true;
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
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (PlayerUI.storage.contents != null)
                {
                    item.slotIndex = 0;

                    if (PlayerUI.storage.AddItem(item))
                    {
                        PlayerUI.inventory.RemoveItem(item);
                    }
                }
                else
                {
                    if (item.Use(GameManager.character, _inventory))
                    {
                        PlayerUI.tooltip.Deactivate();
                    }
                }
            }
        }
    }
}