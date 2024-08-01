using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float detectionRadius = 0.5f;
    private Vector3 _direction;

    public void Initialize(Vector3 initialDirection)
    {
        _direction = initialDirection;
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
                break;
            }
        }
    }

    private void OnBecameInvisible()
    {
        BulletPool.Instance.ReturnBullet(this);
    }
}
