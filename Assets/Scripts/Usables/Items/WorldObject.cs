using UnityEngine;

public class WorldObject : MonoBehaviour
{
    public Item item;
    public bool onGround;

    [SerializeField]BoxCollider _boxCollider = null;
    [SerializeField]MeshCollider _meshCollider = null;
    [SerializeField]Rigidbody _rigidBody = null;

    public string id
    {
        get { return GetComponent<UniqueId>().uniqueId; }
    }

    void Start()
    {
        if (onGround)
        {
            _boxCollider.enabled = true;
            _meshCollider.enabled = true;
            _rigidBody.isKinematic = false;
        }
        else
        {
            _boxCollider.enabled = false;
            _meshCollider.enabled = false;
            _rigidBody.isKinematic = true;
        }
    }
}