using QuangDM.Common;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float detectionRadius = 0.5f;
    private Vector3 _direction;

    private BulletPool _bulletPool;
    public void Initialize(Vector3 initialDirection, BulletPool bulletPool)
    {
        _direction = initialDirection;
        _bulletPool = bulletPool;
    }

    private void Update()
    {
        transform.position += _direction * speed * Time.deltaTime;
        CheckForCollision();
    }

    private void CheckForCollision()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.gameObject.SetActive(false);
                //Observer.Instance.Notify("EnemyDisabled", 1);
                break;
            }
        }
    }

    private void OnBecameInvisible()
    {
        _bulletPool.ReturnBullet(this);
    }
}
