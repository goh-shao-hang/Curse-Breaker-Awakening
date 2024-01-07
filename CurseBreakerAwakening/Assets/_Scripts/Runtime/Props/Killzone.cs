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

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
        {
            if (_teleportsPlayer)
            {
                collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(_damageToPlayer);
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
            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(99);
        }
    }

    private void Update()
    {
        if (_playerDamageable == null)
            return;

        if (_playerDamageTick <= 0f)
        {
            _playerDamageable.TakeDamage(_damageToPlayer);
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
