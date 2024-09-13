using QuangDM.Common;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _direction;
    public void Initialize(Vector3 initialDirection)
    {
        _direction = initialDirection;
    }

    private void FixedUpdate()
    {
        MoveBullet();
    }
    private void MoveBullet()
    {
        transform.position += _direction * Player.Instance.bulletSpeed * Time.deltaTime;

        Quaternion desiredRotation = Quaternion.LookRotation(_direction);
        Quaternion adjustedRotation = desiredRotation * Quaternion.Euler(0, 90, 0);
        transform.rotation = adjustedRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other != null && other.gameObject.activeInHierarchy)
            {
                other.gameObject.GetComponent<EnemyHealth>().TakeDamage(Player.Instance.rangeDamage);
            }
        }
    }

    private void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }
}
