using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class AurasEffect : MonoBehaviour
    {
        public Effect effect;

        [SerializeField]Text _durationTimer = null;

        void Update()
        {
            _durationTimer.text = (Mathf.CeilToInt(effect.duration - effect.durationTimer)).ToString();
        }
    }
}