using QuangDM.Common;
using System;
using UnityEngine;

public abstract class Loot : MonoBehaviour
{
    private float yPositionOffset;

    public virtual void Initialize(Vector3 position)
    {
        if (Mathf.Approximately(yPositionOffset, 0f))
        {
            yPositionOffset = transform.position.y;
        }

        Vector3 newPosition = new Vector3(position.x, position.y + yPositionOffset, position.z);
        transform.position = newPosition;

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
            ObjectPooling.Instance.ReturnObject(this.gameObject);
            Invoke(nameof(RemoveObserver), 0f);
        }
    }

    private void RemoveObserver()
    {
        Observer.Instance.RemoveObserver(EventName.DisableAllLoot, DisableAllLoot);
    }
}
