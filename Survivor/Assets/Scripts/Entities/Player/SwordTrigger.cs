using DG.Tweening;
using QuangDM.Common;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
    private Collider slashCollider;
    private List<EnemyHealth> enemiesInRange = new List<EnemyHealth>();
    private Vector3 originalPos;

    private void Awake()
    {
        slashCollider = GetComponent<Collider>();
        originalPos = transform.localPosition;
    }

    private void Update()
    {
        if (Player.Instance.GetComponent<PlayerController>().magnitude >= 0.1f)
        {
            Vector3 newPosition = transform.localPosition;
            newPosition.z = 1.5f;
            transform.localPosition = newPosition;
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null && !enemiesInRange.Contains(enemyHealth))
            {
                enemiesInRange.Add(enemyHealth);
                Observer.Instance.Notify(EventName.Slash, this);
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
