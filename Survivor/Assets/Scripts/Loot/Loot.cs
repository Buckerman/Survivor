using QuangDM.Common;
using System;
using UnityEngine;

public abstract class Loot : MonoBehaviour
{
    protected LootPool lootPool;
    public virtual void Initialize(Vector3 position, LootPool pool)
    {
        Vector3 newPosition = new Vector3(position.x, transform.position.y, position.z);
        transform.position = newPosition;
        lootPool = pool;

        Observer.Instance.AddObserver(EventName.DisableAllLoot, DisableAllLoot);
    }

    private void DisableAllLoot(object data)
    {
        ReturnToPool();
    }

    public void ReturnToPool()
    {
        Invoke(nameof(RemoveObserver), 0f);
        lootPool.ReturnLoot(this);
    }
    private void RemoveObserver()
    {
        Observer.Instance.RemoveObserver(EventName.DisableAllLoot, DisableAllLoot);
    }
}
