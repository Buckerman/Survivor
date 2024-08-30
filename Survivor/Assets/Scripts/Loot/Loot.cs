using QuangDM.Common;
using System;
using UnityEngine;

public abstract class Loot : MonoBehaviour
{
    protected LootPool lootPool;
    private float yPositionOffset;

    public virtual void Initialize(Vector3 position, LootPool pool)
    {
        if (Math.Abs(yPositionOffset) < Mathf.Epsilon)
        {
            yPositionOffset = transform.position.y;
        }

        Vector3 newPosition = new Vector3(position.x, position.y + yPositionOffset, position.z);
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
        if (this != null)
        {
            Invoke(nameof(RemoveObserver), 0f);
            lootPool.ReturnLoot(this);
        }
    }

    private void RemoveObserver()
    {
        Observer.Instance.RemoveObserver(EventName.DisableAllLoot, DisableAllLoot);
    }
}
