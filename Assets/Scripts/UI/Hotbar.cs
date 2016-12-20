using UnityEngine;

namespace RPG.UI
{
    public class Hotbar : MonoBehaviour
    {
        RPG.Hotbar _hotbar;
        [SerializeField]HotbarSlot _hotbarSlotPrefab = null;

        void Awake()
        {
            _hotbar = GameManager.character.GetComponent<RPG.Hotbar>();
        }

        void Start()
        {
            for (int i = 0; i < _hotbar.usables.Length; i++)
            {
                HotbarSlot hotbarSlot = Instantiate<HotbarSlot>(_hotbarSlotPrefab);
                hotbarSlot.transform.SetParent(transform);
                hotbarSlot.slotIndex = i;
            }
        }
    }
}