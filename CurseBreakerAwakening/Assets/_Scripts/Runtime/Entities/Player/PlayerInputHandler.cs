using CBA.Input;
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
        public event Action OnSprintPressedInput;
        public event Action OnSprintReleasedInput;
        public event Action OnJumpInput;
        public event Action OnCrouchPressedInput;
        public event Action OnCrouchReleasedInput;
        public event Action OnSlidePressedInput;
        public event Action OnSlideReleasedInput;
        public event Action OnAttackPressedInput;
        public event Action OnAttackReleasedInput;

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
            PlayerControls.Gameplay.Sprint.performed += ctx => OnSprintPressed(ctx);
            PlayerControls.Gameplay.Sprint.canceled += ctx => OnSprintReleased(ctx);
            PlayerControls.Gameplay.Jump.performed += ctx => OnJump(ctx);
            PlayerControls.Gameplay.Crouch.performed += ctx => OnCrouchPressed(ctx);
            PlayerControls.Gameplay.Crouch.canceled += ctx => OnCrouchReleased(ctx);
            PlayerControls.Gameplay.Slide.performed += ctx => OnSlidePressed(ctx);
            PlayerControls.Gameplay.Slide.canceled += ctx => OnSlideReleased(ctx);
            PlayerControls.Gameplay.Attack.performed += ctx => OnAttackPressed(ctx);
            PlayerControls.Gameplay.Attack.canceled += ctx => OnAttackReleased(ctx);
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext ctx)
        {
            LookInput = ctx.ReadValue<Vector2>();
        }

        private void OnSprintPressed(InputAction.CallbackContext ctx)
        {
            OnSprintPressedInput?.Invoke();
        }

        private void OnSprintReleased(InputAction.CallbackContext ctx)
        {
            OnSprintReleasedInput?.Invoke();
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            OnJumpInput?.Invoke();
        }

        private void OnCrouchPressed(InputAction.CallbackContext ctx)
        {
            OnCrouchPressedInput?.Invoke();
        }

        private void OnCrouchReleased(InputAction.CallbackContext ctx)
        {
            OnCrouchReleasedInput?.Invoke();
        }

        private void OnSlidePressed(InputAction.CallbackContext ctx)
        {
            OnSlidePressedInput?.Invoke();
        }

        private void OnSlideReleased(InputAction.CallbackContext ctx)
        {
            OnSlideReleasedInput?.Invoke();
        }

        private void OnAttackPressed(InputAction.CallbackContext ctx)
        {
            OnAttackPressedInput?.Invoke();
        }

        private void OnAttackReleased(InputAction.CallbackContext ctx)
        {
            OnAttackReleasedInput?.Invoke();
        }
    }

}