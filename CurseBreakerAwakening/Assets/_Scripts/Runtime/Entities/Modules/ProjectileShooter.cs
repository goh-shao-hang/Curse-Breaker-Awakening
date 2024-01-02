using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private LayerMask _targetLayers;

    [Header("Pooling")]
    [SerializeField] private int _growAmount = 10;

    public ObjectPool<Projectile> ProjectilePool { get; private set; }

    private void Awake()
    {
        ProjectilePool = new ObjectPool<Projectile>(_projectile, _growAmount);
        ProjectilePool.GrowPool();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        FireProjectile(transform.forward);
    }

    public void FireProjectile(Vector3 direction)
    {
        Projectile projectile = ProjectilePool.GetFromPool().Initialize(gameObject, ProjectilePool, _firePoint.position, direction, _targetLayers);
    }
}
