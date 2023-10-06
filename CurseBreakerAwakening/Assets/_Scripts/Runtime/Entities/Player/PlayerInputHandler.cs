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
        private PlayerControls _playerControls;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public event Action OnSprintPressedInput;
        public event Action OnSprintReleasedInput;
        public event Action OnJumpInput;


        private void OnEnable()
        {
            if (_playerControls == null)
            {
                _playerControls = new PlayerControls();
                BindInput();
            }

            _playerControls.Enable();

        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        private void BindInput()
        {
            _playerControls.Gameplay.Move.performed += ctx => OnMove(ctx);
            _playerControls.Gameplay.Look.performed += ctx => OnLook(ctx);
            _playerControls.Gameplay.Sprint.performed += ctx => OnSprintPressed(ctx);
            _playerControls.Gameplay.Sprint.canceled += ctx => OnSprintReleased(ctx);
            _playerControls.Gameplay.Jump.performed += ctx => OnJump(ctx);
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
    }

}