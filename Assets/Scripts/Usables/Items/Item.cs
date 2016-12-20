using UnityEngine;
using RPG;

public class Item : Usable
{
    public int slotIndex;
    public int stack;

    public GameObject modelPrefab { get; protected set; }

    public GameObject worldModel { get; protected set; }

    public float weight { get; protected set; }

    public int stackSize { get; protected set; }

    public Slot slot { get; protected set; }

    public Item()
    {
        modelPrefab = Resources.Load<GameObject>("Items/Shortsword");
        worldModel = null;
        weight = 10;
        slotIndex = 0;
        stackSize = 1;
        stack = 1;
        slot = Slot.None;
    }

    public virtual void Show(GameObject mount)
    {
        worldModel = MonoBehaviour.Instantiate<GameObject>(modelPrefab);
        worldModel.transform.SetParent(mount.transform);
        worldModel.transform.localPosition = Vector3.zero;
        worldModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
        worldModel.transform.localScale = Vector3.one;
        worldModel.GetComponent<WorldObject>().item = this;
        worldModel.GetComponent<WorldObject>().onGround = false;
    }

    public virtual void Drop(Inventory inventory)
    {
        worldModel = MonoBehaviour.Instantiate<GameObject>(modelPrefab);
        worldModel.transform.localPosition = inventory.transform.position + inventory.transform.forward + Vector3.up;
        worldModel.transform.localRotation = inventory.transform.rotation;
        worldModel.transform.localScale = inventory.transform.localScale;
        worldModel.GetComponent<WorldObject>().item = this;
        worldModel.GetComponent<WorldObject>().onGround = true;
        worldModel.AddComponent<UniqueId>().uniqueId = System.Guid.NewGuid().ToString(); //FIXME

        inventory.RemoveItem(this);
    }

    public virtual bool Use(Character character, Inventory inventory)
    {
        return false;
    }

    public virtual bool Unequip(Character character, Inventory inventory)
    {
        return false;
    }
}