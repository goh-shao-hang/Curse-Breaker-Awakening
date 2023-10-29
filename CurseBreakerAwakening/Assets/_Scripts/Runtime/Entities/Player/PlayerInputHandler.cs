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
            PlayerControls.Gameplay.Jump.performed += ctx => OnJump(ctx);
            PlayerControls.Gameplay.Sprint.performed += ctx => OnSprintPressed(ctx);
            PlayerControls.Gameplay.Sprint.canceled += ctx => OnSprintReleased(ctx);
            PlayerControls.Gameplay.Attack.performed += ctx => OnAttack(ctx);
            PlayerControls.Gameplay.Block.performed += ctx => OnBlock(ctx);
            PlayerControls.Gameplay.Kick.performed += ctx => OnKick(ctx);
        }

        private void Update()
        {
        }

        private void LateUpdate()
        {
            ResetButtonInput();
        }

        private void ResetButtonInput()
        {
            JumpInput = false;
            //SprintInput = false;
            AttackInput = false;
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
            JumpInput = true;
        }

        private void OnSprintPressed(InputAction.CallbackContext ctx)
        {
            SprintInput = true;
        }

        private void OnSprintReleased(InputAction.CallbackContext ctx)
        {
            SprintInput = false;
        }

        private void OnAttack(InputAction.CallbackContext ctx)
        {
            AttackInput = true;
        }

        private void OnBlock(InputAction.CallbackContext ctx)
        {
            BlockInput = true;
        }

        private void OnKick(InputAction.CallbackContext ctx)
        {
            KickInput = true;
        }
    }

}