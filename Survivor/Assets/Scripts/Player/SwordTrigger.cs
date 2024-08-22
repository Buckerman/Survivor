using QuangDM.Common;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
    private Collider slashCollider;
    private List<EnemyHealth> enemiesInRange = new List<EnemyHealth>();

    private void Awake()
    {
        slashCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null && !enemiesInRange.Contains(enemyHealth))
            {
                enemiesInRange.Add(enemyHealth);
                Observer.Instance.Notify("Slash", this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null && enemiesInRange.Contains(enemyHealth))
            {
                enemiesInRange.Remove(enemyHealth);
            }
        }
    }

    public List<EnemyHealth> GetEnemiesInRange()
    {
        return enemiesInRange;
    }

    public void ClearEnemiesInRange()
    {
        enemiesInRange.Clear();
    }
}
