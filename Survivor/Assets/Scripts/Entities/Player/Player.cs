using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public int _level;
    [Header("Movement")]
    public float movementSpeed = 5f;
    public float rotationSpeed = 20f;
    public float climbSpeed = 3f;
    [Header("Melee")]
    public float attackRange = 2f;
    public float attackDamage = 12f;
    public float detectionAngle = 120f;
    [Header("Range")]
    public float rangeDamage = 12f;
    public float bulletSpeed = 10f;
    public float shootingRange = 10f;
    public float shootingSpeed = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
