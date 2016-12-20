using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class EquipmentItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Item item;
        public Transform startParent;

        RPG.Inventory _inventory;
        Image _image;
        CanvasGroup _canvasGroup;

        void Awake()
        {
            _inventory = GameManager.character.GetComponent<RPG.Inventory>();
            _image = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update()
        {
            _image.sprite = item.icon;
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

                if (item.Unequip(GameManager.character, _inventory))
                {
                    item.slotIndex = 0;
                    item.Drop(_inventory);

                    PlayerUI.tooltip.Deactivate();
                }
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

                if (item.Unequip(GameManager.character, _inventory))
                {
                    PlayerUI.tooltip.Deactivate();
                }
            }
        }
    }
}