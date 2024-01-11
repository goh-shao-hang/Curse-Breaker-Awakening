using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerWallRunState : PlayerMovementBaseState
    {
        public PlayerWallRunState(EPlayerMovementState key, PlayerController playerController) : base(key, playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _playerController.MovementModule.SetUseGravity(false);

            bool _wallOnRight = (_playerController.CameraRootTransform.forward).magnitude >
                (_playerController.CameraRootTransform.forward + Vector3.Cross(_playerController.runnableWallHitInfo.normal, _playerController.transform.up)).magnitude;

            _playerController.WallRunStarted(_wallOnRight);
        }

        public override void Update()
        {
            base.Update();

            //Apply custom gravity
            _playerController.MovementModule.AddForce(Vector3.down * _playerController.WallRunGravity, ForceMode.Force);

            //Consume Stamina
            ConsumeStamina();

            //Wall jump
            if (_playerController.JumpBuffer)
            {
                PerformWallJump();
            }
        }

        private void ConsumeStamina()
        {
            _playerController.SetStamina(_playerController.CurrentStamina - _playerController.WallRunStaminaConsumption * Time.deltaTime);
            _playerController.StartStaminaRegenTimer();
        }

        private void PerformWallJump()
        {
            _playerController.ConsumeJumpBuffer();

            Vector3 wallJumpDir = _playerController.transform.up + _playerController.runnableWallHitInfo.normal;
            _playerController.MovementModule.SetYVelocity(0f);
            _playerController.MovementModule.AddForce(wallJumpDir * 5f, ForceMode.Impulse);
        }

        public override void Exit()
        {
            base.Exit();

            _playerController.MovementModule.SetUseGravity(true);
            _playerController.WallRunEnded();
        }

        public override EPlayerMovementState GetNextState()
        {
            if (_playerController.IsPerformingChargedAttack)
            {
                return EPlayerMovementState.ChargedAttack;
            }

            if (!_playerController.IsRunnableWallDetected || _playerController.CurrentStamina <= 0f)// || Mathf.Abs(_playerController.PlayerInputHandler.MoveInput.y) < 0.1f)
            {
                return EPlayerMovementState.Walk;
            }

            return this.StateKey;
        }
    }
}