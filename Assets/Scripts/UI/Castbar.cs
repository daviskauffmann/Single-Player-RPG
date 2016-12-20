using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class Castbar : MonoBehaviour
    {
        RPG.Spellbook _spellbook;
        [SerializeField]GameObject _fillBar = null;
        CanvasGroup _canvasGroup;

        void Awake()
        {
            _spellbook = GameManager.character.GetComponent<RPG.Spellbook>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update()
        {
            if (_spellbook.prevSpell != null && !_spellbook.targetingSpell && _spellbook.castTimer < _spellbook.prevSpell.castTime)
            {
                _fillBar.transform.localScale = new Vector3(_spellbook.castTimer / _spellbook.prevSpell.castTime, 1f, 1f);

                _canvasGroup.alpha = 1;
            }
            else
            {
                _canvasGroup.alpha = 0;
            }
        }
    }
}