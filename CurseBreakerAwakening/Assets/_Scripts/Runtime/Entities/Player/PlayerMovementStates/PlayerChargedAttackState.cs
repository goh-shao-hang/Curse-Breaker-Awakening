using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerChargedAttackState : PlayerMovementBaseState
    {
        private float _timeElapsed;

        public PlayerChargedAttackState(EPlayerMovementState key, PlayerController playerController) : base(key, playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _timeElapsed = 0f;

            Debug.Log("charge attacking!");
        }

        public override void Update()
        {
            base.Update();

            _timeElapsed += Time.deltaTime;
        }

        public override void Exit()
        {
            base.Exit();

        }

        public override EPlayerMovementState GetNextState()
        {

            return this.StateKey;
        }
    }
}