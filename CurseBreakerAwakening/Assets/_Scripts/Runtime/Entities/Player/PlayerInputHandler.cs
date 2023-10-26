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
        public bool JumpInput { get; private set; }
        public bool SprintInput { get; private set; }
        public bool AttackInput { get; private set; }
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
        }

        private void Update()
        {
            UpdateButtonInput();
        }

        private void UpdateButtonInput()
        {
            JumpInput = PlayerControls.Gameplay.Jump.WasPressedThisFrame();
            SprintInput = PlayerControls.Gameplay.Sprint.WasPressedThisFrame();
            AttackInput = PlayerControls.Gameplay.Attack.WasPressedThisFrame();
            BlockInput = PlayerControls.Gameplay.Block.WasPressedThisFrame();
            KickInput = PlayerControls.Gameplay.Kick.WasPressedThisFrame();
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext ctx)
        {
            LookInput = ctx.ReadValue<Vector2>();
        }

    }

}