using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class HotbarUsable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Usable usable;
        public Transform startParent;

        [SerializeField]Text _stack = null;
        Image _image;
        CanvasGroup _canvasGroup;
        RPG.Inventory _inventory;
        RPG.Spellbook _spellbook;
        RPG.Hotbar _hotbar;

        void Awake()
        {
            _image = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _inventory = GameManager.character.GetComponent<RPG.Inventory>();
            _spellbook = GameManager.character.GetComponent<RPG.Spellbook>();
            _hotbar = GameManager.character.GetComponent<RPG.Hotbar>();
        }

        void Update()
        {
            _image.sprite = usable.icon;

            if (usable is Item)
            {
                Item item = (Item)usable;

                if (!_inventory.items.Contains(item))
                {
                    usable = null;
                    gameObject.SetActive(false);
                }

                if (item.stack > 1)
                {
                    _stack.text = item.stack.ToString();
                }
                else
                {
                    _stack.text = null;
                }
            }

            if (usable is Spell)
            {
                Spell spell = (Spell)usable;

                if (!_spellbook.spells.Contains(spell))
                {
                    usable = null;
                    gameObject.SetActive(false);
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _hotbar.usables[GetComponentInParent<HotbarSlot>().slotIndex] = null;

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

                usable = null;
                gameObject.SetActive(false);
            }

            _canvasGroup.blocksRaycasts = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayerUI.tooltip.Activate(usable);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayerUI.tooltip.Deactivate();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
            {
                if (usable is Item)
                {
                    Item item = (Item)usable;

                    if (item.Use(GameManager.character, _inventory))
                    {
                        PlayerUI.tooltip.Deactivate();
                    }
                }

                if (usable is Spell)
                {
                    Spell spell = (Spell)usable;
                    _spellbook.CastSpell(spell);
                }
            }
        }
    }
}