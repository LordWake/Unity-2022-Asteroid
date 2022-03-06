using UnityEngine;

public class PoolContainerManager : MonoBehaviour
{
    public void SaveObjectInContainer(GameObject newObject)
    {
        newObject.transform.parent = transform;
    }
}
