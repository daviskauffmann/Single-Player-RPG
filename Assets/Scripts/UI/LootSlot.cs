using UnityEngine;
using UnityEngine.UI;

public class LootSlot : MonoBehaviour
{
    public Item item;
    public GameObject slot;

    [SerializeField]Text _name = null;

    void Start()
    {
        _name.text = item.name;
    }
}