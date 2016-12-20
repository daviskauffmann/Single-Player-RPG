using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class Auras : MonoBehaviour
    {
        [SerializeField]AurasEffect _aurasEffectPrefab = null;

        public void AddEffect(Effect effect)
        {
            GameManager.character.effects.Add(effect);

            AurasEffect aurasEffect = Instantiate<AurasEffect>(_aurasEffectPrefab);
            aurasEffect.transform.SetParent(transform);
            aurasEffect.GetComponent<Image>().sprite = effect.icon;
            aurasEffect.effect = effect;
            aurasEffect.effect.uiObject = aurasEffect.gameObject;
        }

        public void RemoveEffect(Effect effect)
        {
            GameManager.character.effects.Remove(effect);

            Destroy(effect.uiObject);
        }
    }
}