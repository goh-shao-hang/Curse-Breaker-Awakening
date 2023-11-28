using GameCells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCells.StateMachine
{
    public class Transition
    {
        public State TargetState { get; }
        public Condition Condition { get; }

        public Transition(State targetState, Condition condition)
        {
            TargetState = targetState;
            Condition = condition;
        }
    }
}