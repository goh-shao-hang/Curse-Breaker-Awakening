using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerChargedAttackState : PlayerMovementBaseState
    {
        private float _chargedAttackMovementSpeed;
        private Vector3 _chargedAttackDirection;
        private bool _chargeAttackEnded;

        public PlayerChargedAttackState(EPlayerMovementState key, PlayerController playerController) : base(key, playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _chargeAttackEnded = false;

            _playerController.PlayerCombatManager.OnChargedAttackEnded += OnChargeAttackEnded;

            Debug.Log("charge attacking!");
            _playerController.SetMovementForce(0f);

            _chargedAttackMovementSpeed = _playerController.MinChargedAttackMovementSpeed +
                (_playerController.MaxChargedAttackMovementSpeed - _playerController.MinChargedAttackMovementSpeed) * _playerController.LastChargePercentage;

            _chargedAttackDirection = _playerController.CameraRootTransform.forward;

            _playerController.MovementModule.SetVelocity(_chargedAttackDirection * _chargedAttackMovementSpeed);
            _playerController.SetIsLimitingMaxSpeed(false);
        }

        private void OnChargeAttackEnded()
        {
            _chargeAttackEnded = true;
        }

        public override void Update()
        {
            base.Update();

            _playerController.MovementModule.SetVelocity(_chargedAttackDirection * _chargedAttackMovementSpeed);
        }

        public override void Exit()
        {
            base.Exit();

            _playerController.PlayerCombatManager.OnChargedAttackEnded -= OnChargeAttackEnded;

            _playerController.SetIsLimitingMaxSpeed(true);
            Debug.Log("charge attack end");
        }

        public override EPlayerMovementState GetNextState()
        {
            if (_chargeAttackEnded)
            {
                return EPlayerMovementState.Walk;
            }

            return this.StateKey;
        }
    }
}