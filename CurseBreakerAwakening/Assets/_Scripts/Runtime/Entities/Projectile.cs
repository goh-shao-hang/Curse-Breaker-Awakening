using CBA;
using CBA.Entities;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header(GameData.DEPENDENCIES)]
    [SerializeField] private Rigidbody _rigidbody;

    [Header(GameData.SETTINGS)]
    [SerializeField] private float _damage = 20f;
    [SerializeField] private float _lifetime = 5f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private bool _pierce = false;

    private RangedAttack _owner;
    private LayerMask _targetLayers;

    private Coroutine _lifetimeCO;

    public Projectile Initialize(RangedAttack owner, Vector3 position, Vector3 direction, LayerMask targetLayers)
    {
        this._owner = owner;
        this.transform.position = position;
        this.transform.forward = direction;
        this._rigidbody.velocity = transform.forward * _speed;
        this._targetLayers = targetLayers;
        return this;
    }

    private void OnEnable()
    {
        if (_lifetimeCO != null)
        {
            StopCoroutine(_lifetimeCO);
        }

        _lifetimeCO = StartCoroutine(LifetimeCO());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _targetLayers) != 0)
        {
            other.GetComponent<IDamageable>()?.TakeDamage(_damage);

            if (_pierce)
                return;

            StopCoroutine(_lifetimeCO);
            _lifetimeCO = null;
            _owner.AddToPool(this);
        }
    }

    private IEnumerator LifetimeCO()
    {
        yield return WaitHandler.GetWaitForSeconds(_lifetime);

        _owner.AddToPool(this);

        _lifetimeCO = null;
    }
}
