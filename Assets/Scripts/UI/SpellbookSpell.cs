using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class SpellbookSpell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Spell spell;
        public Transform startParent;

        RPG.Spellbook _spellbook;
        CanvasGroup _canvasGroup;

        void Awake()
        {
            _spellbook = GameManager.character.GetComponent<RPG.Spellbook>();
            _canvasGroup = GetComponent<CanvasGroup>();
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
                transform.SetParent(startParent, false);
                transform.localPosition = Vector3.zero;
            }

            _canvasGroup.blocksRaycasts = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayerUI.tooltip.Activate((Usable)spell);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayerUI.tooltip.Deactivate();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
            {
                _spellbook.CastSpell(spell);
            }
        }
    }
}