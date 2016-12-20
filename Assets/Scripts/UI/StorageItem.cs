using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class StorageItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public RPG.Inventory contents;
        public Item item;
        public Transform startParent;

        [SerializeField]Text _stack = null;
        CanvasGroup _canvasGroup;

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update()
        {
            if (!contents.items.Contains(item))
            {
                Destroy(gameObject);
            }

            if (GetComponentInParent<StorageSlot>() != null)
            {
                item.slotIndex = GetComponentInParent<StorageSlot>().slotIndex;
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
                PlayerUI.storage.RemoveItem(item);
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
                transform.SetParent(startParent);
                transform.localPosition = Vector3.zero;

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
                item.slotIndex = 0;

                if (PlayerUI.inventory.AddItem(item))
                {
                    PlayerUI.storage.RemoveItem(item);
                }
            }
        }
    }
}