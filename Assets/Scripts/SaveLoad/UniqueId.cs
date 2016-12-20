using UnityEngine;

public class UniqueIdentifierAttribute : PropertyAttribute
{
}

public class UniqueId : MonoBehaviour
{
    [UniqueIdentifier]
    public string uniqueId;
}

