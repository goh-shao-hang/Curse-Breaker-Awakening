using CBA;
using CBA.Core;
using CBA.Entities;
using DG.Tweening;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header(GameData.DEPENDENCIES)]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject _onHitVfx;
    [SerializeField] private AudioEmitter _audioEmitter;
    [SerializeField] private string _collisionSfxName = "Explosion";

    [Header(GameData.SETTINGS)]
    [SerializeField] private float _damage = 20f;
    [SerializeField] private float _lifetime = 5f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _delay = 0f;
    [SerializeField] private bool _pierce = false;

    private GameObject _owner;
    private ObjectPool<Projectile> _pool;
    private LayerMask _targetLayers;

    private DamageData _damageData;

    private SO_GlobalPosition _target; //Delayed / homing only

    private Coroutine _lifetimeCO;

    public Projectile Initialize(GameObject owner, ObjectPool<Projectile> pool, Vector3 position, Vector3 direction, LayerMask targetLayers)
    {
        this._owner = owner;

        this._pool = pool;
        this.transform.position = position;
        this.transform.forward = direction;
        this._targetLayers = targetLayers;

        #region Fire
        this._rigidbody.velocity = transform.forward * _speed;

        if (_lifetimeCO != null)
        {
            StopCoroutine(_lifetimeCO);
        }

        _lifetimeCO = StartCoroutine(LifetimeCO());
        #endregion

        return this;
    }

    public Projectile InitializeWithDelay(GameObject owner, ObjectPool<Projectile> pool, Vector3 position, SO_GlobalPosition target, LayerMask targetLayers, Transform parent = null)
    {
        this._owner = owner;

        this._pool = pool;
        this.transform.position = position;
        this._target = target;
        this._targetLayers = targetLayers;

        if (parent != null)
            this.transform.SetParent(parent);

        this._rigidbody.detectCollisions = false;
        this._rigidbody.velocity = Vector3.zero;

        if (_lifetimeCO != null)
        {
            StopCoroutine(_lifetimeCO);
        }

        StartCoroutine(DelayedFire());

        return this;
    }

    private IEnumerator DelayedFire()
    {
        yield return new WaitForSeconds(_delay);

        this.transform.SetParent(null);
        this._rigidbody.detectCollisions = true;
        this.transform.forward = (_target.Value - transform.position).normalized;
        this._rigidbody.velocity = transform.forward * _speed;

        _lifetimeCO = StartCoroutine(LifetimeCO());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _owner)
            return;

        if (((1 << other.gameObject.layer) & _targetLayers) != 0)
        {
            _damageData.Set(_damage, null);
            other.GetComponent<IDamageable>()?.TakeDamage(_damageData);

            if (_pierce)
                return;

            StopCoroutine(_lifetimeCO);
            _lifetimeCO = null;

            _pool.AddToPool(this);
            gameObject.SetActive(false);

            if (_onHitVfx != null)
            {
                Instantiate(_onHitVfx, transform.position, Quaternion.identity);
            }

            if (_audioEmitter != null)
            {
                _audioEmitter.transform.SetParent(null);
                _audioEmitter.PlayOneShotSfx(_collisionSfxName);
                DOVirtual.DelayedCall(3f, () => _audioEmitter.transform.parent = this.transform);
            }
        }
    }

    private IEnumerator LifetimeCO()
    {
        yield return WaitHandler.GetWaitForSeconds(_lifetime);

        _lifetimeCO = null;

        _pool.AddToPool(this);
        gameObject.SetActive(false);

        if (_onHitVfx != null)
        {
            Instantiate(_onHitVfx, transform.position, Quaternion.identity);
        }

        if (_audioEmitter != null)
        {
            _audioEmitter.transform.SetParent(null);
            _audioEmitter.PlayOneShotSfx(_collisionSfxName);
            DOVirtual.DelayedCall(3f, () => _audioEmitter.transform.parent = this.transform);
        }
    }
}
