using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class Hotbar : MonoBehaviour
    {
        public Usable[] usables = new Usable[10];

        Inventory _inventory;
        Spellbook _spellbook;

        void Awake()
        {
            _inventory = GetComponent<Inventory>();
            _spellbook = GetComponent<Spellbook>();
        }

        void Update()
        {
            for (int i = 0; i < usables.Length; i++)
            {
                if (usables[i] is Item)
                {
                    if (!_inventory.items.Contains((Item)usables[i]))
                    {
                        usables[i] = null;
                    }
                }

                if (usables[i] is Spell)
                {
                    if (!_spellbook.spells.Contains((Spell)usables[i]))
                    {
                        usables[i] = null;
                    }
                }
            }
        }
    }
}