using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class Spellbook : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        RPG.Spellbook _spellbook;
        [SerializeField]GameObject _panel = null;
        [SerializeField]GameObject _spellbookSlotPrefab = null;
        List<GameObject> _slots = new List<GameObject>();
        [SerializeField]SpellbookSpell _spellbookSpellPrefab = null;
        bool _isOpen;
        private Vector3 _offset;
        CanvasGroup _canvasGroup;

        void Awake()
        {
            _spellbook = GameManager.character.GetComponent<RPG.Spellbook>();
            _canvasGroup = GetComponent<CanvasGroup>();
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

            _spellbook.spells.Sort(delegate(Spell a, Spell b)
                {
                    return a.name.CompareTo(b.name); 
                });

            for (int i = 0; i < _spellbook.spells.Count; i++)
            {
                GameObject spellbookSlot = Instantiate<GameObject>(_spellbookSlotPrefab);
                spellbookSlot.transform.SetParent(_panel.transform);
                _slots.Add(spellbookSlot);

                SpellbookSpell spellbookSpell = Instantiate<SpellbookSpell>(_spellbookSpellPrefab);
                spellbookSpell.transform.SetParent(spellbookSlot.transform);
                spellbookSpell.transform.localPosition = Vector3.zero;
                spellbookSpell.GetComponent<Image>().sprite = _spellbook.spells[i].icon;
                spellbookSpell.spell = _spellbook.spells[i];
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

        public void AddSpell(Spell addSpell)
        {
            for (int i = 0; i < _spellbook.spells.Count; i++)
            {
                if (_spellbook.spells[i].GetType() == addSpell.GetType())
                {
                    return;
                }
            }

            _spellbook.spells.Add(addSpell);

            if (_isOpen)
            {
                Open();
            }
        }

        public void RemoveSpell(Spell removeSpell)
        {
            _spellbook.spells.Remove(removeSpell);

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