using CBA;
using CBA.Entities;
using CBA.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    [Header(GameData.SETTINGS)]
    [SerializeField] private float _damageToPlayer = 10f;
    [SerializeField] private float _playerDamageInterval = 1f;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _killLayers;
    [SerializeField] private bool _teleportsPlayer = true;

    private float _playerDamageTick = 0f;

    private IDamageable _playerDamageable = null;
    private DamageData _damageData;

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
        {
            if (_teleportsPlayer)
            {
                _damageData.Set(_damageToPlayer, this.gameObject, false);
                collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(_damageData);
                LevelManager.Instance?.TeleportPlayerToSafePoint();
            }
            else
            {
                _playerDamageTick = _playerDamageInterval;
                _playerDamageable = collision.gameObject.GetComponent<IDamageable>();
                // if not teleporting, the damage is done in Update
            }
        }
        else if (((1 << collision.gameObject.layer) & _killLayers) != 0)
        {
            _damageData.Set(float.MaxValue, this.gameObject, false);
            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(_damageData);
        }
    }

    private void Update()
    {
        if (_playerDamageable == null)
            return;

        if (_playerDamageTick <= 0f)
        {
            _damageData.Set(_damageToPlayer, this.gameObject, false);
            _playerDamageable.TakeDamage(_damageData);
            _playerDamageTick = _playerDamageInterval;
        }
        else
        {
            _playerDamageTick -= Time.deltaTime;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (_teleportsPlayer)
            return;

        _playerDamageTick = 0f;
        _playerDamageable = null;
    }
}
