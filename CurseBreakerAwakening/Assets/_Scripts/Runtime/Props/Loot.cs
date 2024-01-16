using CBA;
using CBA.Core;
using CBA.LevelGeneration;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Loot : MonoBehaviour
{
    [SerializeField] private SO_GlobalPosition _playerPosition;
    [SerializeField] private float _followSpeed = 20f;
    [SerializeField] private string _collectSoundEffectName;

    private Rigidbody _lootRigidbody;
    private Collider _lootCollider;
    private Rigidbody lootRigidbody => _lootRigidbody ??= GetComponent<Rigidbody>();
    private Collider lootCollider => _lootCollider ??= GetComponentInChildren<Collider>();

    private Coroutine _followCO = null;

    private ObjectPool<Loot> _pool;

    public abstract Type GetLootType();

    private void OnEnable()
    {
        lootCollider.isTrigger = false;
        _followCO = StartCoroutine(FollowCO());

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnRoomChanged += InstantCollect;
        }
    }

    private void OnDisable()
    {
        if (_followCO != null)
            StopCoroutine(_followCO);

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnRoomChanged -= InstantCollect;
        }
    }

    private void Update()
    {
        if (_followCO != null)
            return;

        transform.position = Vector3.Lerp(transform.position, _playerPosition.Value, _followSpeed * Time.deltaTime);
    }

    private IEnumerator FollowCO()
    {
        yield return WaitHandler.GetWaitForSeconds(GameData.LOOT_FOLLOW_DELAY);

        lootCollider.isTrigger = true;
        _followCO = null;
    }

    public Loot Initialize(Vector3 launchForce, Vector3 position, ObjectPool<Loot> pool)
    {
        this.lootRigidbody.AddForceAtPosition(launchForce, position, ForceMode.Impulse);
        this._pool = pool;

        return this;
    }

    private void InstantCollect()
    {
        OnCollected(GameManager.Instance.PlayerManager.PlayerController.gameObject);
    }

    protected virtual void OnCollected(GameObject _playerGameObject)
    {
        if (_pool != null)
            _pool.AddToPool(this);
        else
            Destroy(gameObject);

        if (_collectSoundEffectName != string.Empty)
        {
            AudioManager.Instance.PlayGlobalSFX(_collectSoundEffectName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & GameData.PLAYER_LAYER) != 0)
        {
            OnCollected(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & GameData.PLAYER_LAYER) != 0)
        {
            OnCollected(collision.gameObject);
        }
    }
}
