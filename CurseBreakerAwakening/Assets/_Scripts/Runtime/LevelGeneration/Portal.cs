using CBA.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.LevelGeneration
{
    public class Portal : MonoBehaviour
    {
        private bool _collided = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_collided)
                return;

            if (((1 << other.gameObject.layer) & GameData.PLAYER_LAYER) != 0)
            {
                _collided = true;
                GameManager.Instance?.EndLevel();
            }
        }

    }
}