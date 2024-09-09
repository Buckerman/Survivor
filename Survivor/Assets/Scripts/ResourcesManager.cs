using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private Dictionary<string, Object> lstResources = new Dictionary<string, Object>();
    public T Load<T>(string path) where T : Object
    {
        if (!lstResources.ContainsKey(path))
        {
            T go = Resources.Load<T>(path);
            lstResources.Add(path,go);
        }
        return (T)lstResources[path];
    }
}
