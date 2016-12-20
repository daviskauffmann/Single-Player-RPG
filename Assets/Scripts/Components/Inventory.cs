using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.UI;

namespace RPG
{
    public class Inventory : MonoBehaviour
    {
        public List<Item> items = new List<Item>();
        public int slotAmount = 60;

        Character _character;
        [SerializeField]string[] _spawnItems = null;

        void Awake()
        {
            _character = GetComponent<Character>();
        }

        void Start()
        {
            SpawnItems();
        }

        void Update()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Update();
            }
        }

        void SpawnItems()
        {
            for (int i = 0; i < GameManager.instance.data.scenes.Count; i++)
            {
                if (GameManager.instance.data.scenes[i].sceneId == SceneManager.GetActiveScene().buildIndex)
                {
                    if (!GameManager.instance.data.scenes[i].reload)
                    {
                        return;
                    }
                }
            }

            if (_spawnItems != null)
            {
                for (int i = 0; i < _spawnItems.Length; i++)
                {
                    items.Add((Item)Activator.CreateInstance(Type.GetType(_spawnItems[i])));
                }
            }
        }

        public bool AddItem(Item item)
        {
            if (_character == GameManager.character)
            {
                if (!PlayerUI.inventory.AddItem(item))
                {
                    return false;
                }	
            }
            else
            {
                items.Add(item);
            }

            return true;
        }

        public void RemoveItem(Item item)
        {
            if (_character == GameManager.character)
            {
                PlayerUI.inventory.RemoveItem(item);
            }
            else
            {
                items.Remove(item);
            }
        }
    }
}