using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class EquipmentSlot : MonoBehaviour, IDropHandler
    {
        RPG.Inventory _inventory;
        [SerializeField]Slot _slot = Slot.None;
        [SerializeField]EquipmentItem _equipmentItemPrefab = null;
        EquipmentItem _equipmentItem;

        void Awake()
        {
            _inventory = GameManager.character.GetComponent<RPG.Inventory>();
        }

        void Start()
        {
            _equipmentItem = Instantiate<EquipmentItem>(_equipmentItemPrefab);
            _equipmentItem.transform.SetParent(gameObject.transform);
            _equipmentItem.transform.localPosition = Vector3.zero;
            _equipmentItem.gameObject.SetActive(false);
        }

        void Update()
        {
            switch (_slot)
            {
                case Slot.Helmet:
                    if (GameManager.character.equippedHelmet != null)
                    {
                        _equipmentItem.item = GameManager.character.equippedHelmet;
                        _equipmentItem.gameObject.SetActive(true);
                    }
                    else
                    {
                        _equipmentItem.item = null;
                        _equipmentItem.gameObject.SetActive(false);
                    }

                    break;
                case Slot.Cloak:
                    if (GameManager.character.equippedCloak != null)
                    {
                        _equipmentItem.item = GameManager.character.equippedCloak;
                        _equipmentItem.gameObject.SetActive(true);
                    }
                    else
                    {
                        _equipmentItem.item = null;
                        _equipmentItem.gameObject.SetActive(false);
                    }

                    break;
                case Slot.Armor:
                    if (GameManager.character.equippedArmor != null)
                    {
                        _equipmentItem.item = GameManager.character.equippedArmor;
                        _equipmentItem.gameObject.SetActive(true);
                    }
                    else
                    {
                        _equipmentItem.item = null;
                        _equipmentItem.gameObject.SetActive(false);
                    }

                    break;
                case Slot.MainHand:
                    if (GameManager.character.equippedMainHand != null)
                    {
                        _equipmentItem.item = GameManager.character.equippedMainHand;
                        _equipmentItem.gameObject.SetActive(true);
                    }
                    else
                    {
                        _equipmentItem.item = null;
                        _equipmentItem.gameObject.SetActive(false);
                    }

                    break;
                case Slot.OffHand:
                    if (GameManager.character.equippedOffHand != null)
                    {
                        _equipmentItem.item = GameManager.character.equippedOffHand;

                        _equipmentItem.gameObject.SetActive(true);
                    }
                    else
                    {
                        _equipmentItem.item = null;
                        _equipmentItem.gameObject.SetActive(false);
                    }

                    break;
                case Slot.Ranged: 
                    if (GameManager.character.equippedRanged != null)
                    {
                        _equipmentItem.item = GameManager.character.equippedRanged;
                        _equipmentItem.gameObject.SetActive(true);
                    }
                    else
                    {
                        _equipmentItem.item = null;
                        _equipmentItem.gameObject.SetActive(false);
                    }

                    break;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                if (eventData.pointerDrag.GetComponent<InventoryItem>() != null)
                {
                    InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();

                    if (_slot == droppedItem.item.slot)
                    {
                        if (_equipmentItem.gameObject.activeInHierarchy)
                        {
                            _equipmentItem.item.Unequip(GameManager.character, _inventory);
                        }

                        droppedItem.item.Use(GameManager.character, _inventory);
                    }

                    droppedItem.transform.SetParent(droppedItem.startParent);
                    droppedItem.transform.localPosition = Vector3.zero;
                }

                if (eventData.pointerDrag.GetComponent<EquipmentItem>() != null)
                {
                    EquipmentItem droppedItem = eventData.pointerDrag.GetComponent<EquipmentItem>();

                    droppedItem.transform.SetParent(droppedItem.startParent); 
                    droppedItem.transform.localPosition = Vector3.zero;
                }
            }
        }
    }
}