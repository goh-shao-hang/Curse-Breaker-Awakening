using CBA.Input;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


namespace CBA.Entities.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public PlayerControls PlayerControls { get; private set; }

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public event Action OnJumpInput;
        public event Action OnAttackPressedInput;
        public event Action OnAttackReleasedInput;
        public event Action OnInteractPressedInput;

        public bool ChargeInput { get; private set; }
        public bool SprintInput { get; private set; }
        public bool CrouchInput { get; private set; }
        public bool BlockInput { get; private set; }
        public bool KickInput { get; private set; }

        private void OnEnable()
        {
            if (PlayerControls == null)
            {
                PlayerControls = new PlayerControls();
                BindInput();
            }

            PlayerControls.Enable();
        }

        private void OnDisable()
        {
            PlayerControls.Disable();
        }

        private void BindInput()
        {
            PlayerControls.Gameplay.Move.performed += ctx => OnMove(ctx);
            PlayerControls.Gameplay.Look.performed += ctx => OnLook(ctx);
            PlayerControls.Gameplay.Jump.performed += ctx => OnJump(ctx);
            PlayerControls.Gameplay.Sprint.performed += ctx => OnSprintPressed(ctx);
            PlayerControls.Gameplay.Sprint.canceled += ctx => OnSprintReleased(ctx);
            PlayerControls.Gameplay.Crouch.performed += ctx => OnCrouchPressed(ctx);
            PlayerControls.Gameplay.Crouch.canceled += ctx => OnCrouchReleased(ctx);
            PlayerControls.Gameplay.Attack.performed += ctx => OnAttackPressed(ctx);
            PlayerControls.Gameplay.Attack.canceled += ctx => OnAttackReleased(ctx);
            PlayerControls.Gameplay.Block.performed += ctx => OnBlock(ctx);
            PlayerControls.Gameplay.Kick.performed += ctx => OnKick(ctx);
            PlayerControls.Gameplay.Interact.performed += ctx => OnInteractPressed(ctx);
        }

        private void LateUpdate()
        {
            ResetButtonInput();
        }

        private void ResetButtonInput()
        {
            BlockInput = false;
            KickInput = false;
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext ctx)
        {
            LookInput = ctx.ReadValue<Vector2>();
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            OnJumpInput?.Invoke();
        }

        private void OnSprintPressed(InputAction.CallbackContext ctx)
        {
            SprintInput = true;
        }

        private void OnSprintReleased(InputAction.CallbackContext ctx)
        {
            SprintInput = false;
        }

        private void OnCrouchPressed(InputAction.CallbackContext ctx)
        {
            CrouchInput = true;
        }

        private void OnCrouchReleased(InputAction.CallbackContext ctx)
        {
            CrouchInput = false;
        }

        private void OnAttackPressed(InputAction.CallbackContext ctx)
        {
            OnAttackPressedInput?.Invoke();
        }

        private void OnAttackReleased(InputAction.CallbackContext ctx)
        {
            OnAttackReleasedInput?.Invoke();
        }

        private void OnBlock(InputAction.CallbackContext ctx)
        {
            BlockInput = true;
        }

        private void OnKick(InputAction.CallbackContext ctx)
        {
            KickInput = true;
        }

        private void OnInteractPressed(InputAction.CallbackContext ctx)
        {
            OnInteractPressedInput?.Invoke();
        }
    }

}