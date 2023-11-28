using GameCells;
using GameCells.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class Condition_GPressed : Condition
    {
        public override bool Evaluate()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.G);
        }
    }
}