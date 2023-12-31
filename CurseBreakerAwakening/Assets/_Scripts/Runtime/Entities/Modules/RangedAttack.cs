using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private LayerMask _targetLayers;

    [Header("Pooling")]
    [SerializeField] private int _growAmount = 10;

    public Queue<Projectile> ProjectilePool { get; private set; } = new Queue<Projectile>();

    private void Awake()
    {
        GrowPool();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        FireProjectile(transform.forward);
    }

    private void GrowPool()
    {
        for (int i = 0; i < _growAmount; i++)
        {
            var projectile = Instantiate(_projectile);
            AddToPool(projectile);
        }
    }

    public void AddToPool(Projectile projectileInstance)
    {
        projectileInstance.gameObject.SetActive(false);
        ProjectilePool.Enqueue(projectileInstance);
    }

    public void FireProjectile(Vector3 direction)
    {
        if (ProjectilePool.Count == 0)
        {
            GrowPool(); 
        }

        Projectile projectile = ProjectilePool.Dequeue().Initialize(this, _firePoint.position, direction, _targetLayers);
        projectile.gameObject.SetActive(true);
    }
}
