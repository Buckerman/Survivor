using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (Instance.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
        {
            Destroy(this.gameObject);
        }
    }

    Dictionary<GameObject, List<GameObject>> _poolObjects = new Dictionary<GameObject, List<GameObject>>();

    public GameObject GetObject(GameObject key)
    {
        List<GameObject> _itemPool = new List<GameObject>();
        if (!_poolObjects.ContainsKey(key))
        {
            _poolObjects.Add(key, _itemPool);
        }
        else
        {
            _itemPool = _poolObjects[key];
        }

        foreach (GameObject g in _itemPool)
        {
            if (!g.gameObject.activeSelf) // Find an inactive object
            {
                g.SetActive(true); // Activate the object
                return g; // Return the reused object
            }
        }

        // If no inactive object is found, instantiate a new one
        GameObject g2 = Instantiate(key);
        _poolObjects[key].Add(g2);
        return g2;
    }

    // This method returns an object to the pool by deactivating it
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false); // Deactivate the object
    }

    public void ClearPool()
    {
        _poolObjects.Clear(); // Clear the pool dictionary
    }
}
