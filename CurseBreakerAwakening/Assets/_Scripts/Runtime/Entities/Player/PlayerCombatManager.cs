using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerCombatManager : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private Transform _weaponHolderTransform;
        [SerializeField] private Animator _weaponAnimator;

        [Header(GameData.SETTINGS)]
        [SerializeField] private float _attackRange = 3f;
        [SerializeField] private float _attackDelay = 0.4f;
        [SerializeField] private float _attackSpeed = 1f;
        [SerializeField] private float _attackDamage = 1f;
        [SerializeField] private LayerMask _attackLayer;

        [Header("Extras")]
        [SerializeField] private GameObject _hitEffect;
        [SerializeField] private AudioSource _playerAudioSource;
        [SerializeField] private AudioClip _attackSfx;
        [SerializeField] private AudioClip _hitSfx;

        private bool _isAttacking = false;
        private bool _readyToAttack = true;
        private int _currentCombo;

        private void OnEnable()
        {
            _playerInputHandler.OnAttackPressedInput += Attack;
        }

        private void OnDisable()
        {
            _playerInputHandler.OnAttackPressedInput -= Attack;
        }

        private void LateUpdate()
        {
            _weaponHolderTransform.transform.rotation = Helper.MainCamera.transform.rotation;

        }

        private void Update()
        {
            
        }

        private void Attack()
        {
            if (!_readyToAttack || _isAttacking)
                return;

            _readyToAttack = false;
            _isAttacking = true;

            Invoke(nameof(ResetAttack), _attackSpeed);
            Invoke(nameof(AttackRaycast), _attackDelay);

            if (_playerAudioSource != null)
            {
                //TODO
            }
        }

        private void AttackRaycast()
        {
            if (Physics.Raycast(Helper.MainCamera.transform.position, Helper.MainCamera.transform.forward, out RaycastHit hit, _attackRange, _attackLayer))
            {
                HitTarget(hit.point);
            }
        }

        private void HitTarget(Vector3 hitPosition)
        {
            
        }

        private void ResetAttack()
        {
            _isAttacking = false;
            _readyToAttack = true;
        }
    }
}