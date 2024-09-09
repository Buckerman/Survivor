using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling: MonoBehaviour
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
            if (g.gameObject.activeSelf)
                continue;
            return g;
        }

        GameObject g2 = Instantiate(key);
        _poolObjects[key].Add(g2);
        return g2;
    }
    public void ClearPool()
    {
        _poolObjects.Clear();
    }
}
