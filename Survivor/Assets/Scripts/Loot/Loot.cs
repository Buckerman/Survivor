using UnityEngine;

public abstract class Loot : MonoBehaviour
{
    protected LootPool lootPool;
    public virtual void Initialize(Vector3 position, LootPool pool)
    {
        Vector3 newPosition = new Vector3(position.x, transform.position.y, position.z);
        transform.position = newPosition;
        lootPool = pool;
    }

    public void ReturnToPool()
    {
        lootPool.ReturnLoot(this);
    }
}
