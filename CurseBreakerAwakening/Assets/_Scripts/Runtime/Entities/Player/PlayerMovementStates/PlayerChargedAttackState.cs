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

            _playerController.SetMovementForce(0f);

            //Allow player to go through enemies. Another dedicated hitbox will be used to detect hits
            Physics.IgnoreLayerCollision(GameData.PLAYER_LAYER_INDEX, GameData.ENEMY_LAYER_INDEX, true);

            _chargedAttackMovementSpeed = Mathf.Lerp(_playerController.MinChargedAttackMovementSpeed, _playerController.MaxChargedAttackMovementSpeed, 
                _playerController.LastChargePercentage);

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

            Physics.IgnoreLayerCollision(GameData.PLAYER_LAYER_INDEX, GameData.ENEMY_LAYER_INDEX, false);

            _playerController.SetIsLimitingMaxSpeed(true);

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