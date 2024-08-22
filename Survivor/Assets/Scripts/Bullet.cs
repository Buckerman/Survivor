using QuangDM.Common;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletDamage = 12f;
    private Vector3 _direction;

    private BulletPool _bulletPool;
    public void Initialize(Vector3 initialDirection, BulletPool bulletPool)
    {
        _direction = initialDirection;
        _bulletPool = bulletPool;
    }

    private void FixedUpdate()
    {
        MoveBullet();
    }
    private void MoveBullet()
    {
        transform.position += _direction * bulletSpeed * Time.deltaTime;

        Quaternion desiredRotation = Quaternion.LookRotation(_direction);
        Quaternion adjustedRotation = desiredRotation * Quaternion.Euler(0, 90, 0);
        transform.rotation = adjustedRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.gameObject.activeInHierarchy)
            {
                other.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);
            }
        }
    }

    private void OnBecameInvisible()
    {
        _bulletPool.ReturnBullet(this);
    }
}
