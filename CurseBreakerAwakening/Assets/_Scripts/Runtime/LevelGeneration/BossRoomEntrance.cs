using CBA.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.LevelGeneration
{
    public class BossRoomEntrance : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (((1 << collision.collider.gameObject.layer) & GameData.PLAYER_LAYER) != 0)
            {
                GameManager.Instance?.EnterBossRoom();
            }
        }
    }
}