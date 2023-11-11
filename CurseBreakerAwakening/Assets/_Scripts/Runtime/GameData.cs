using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA
{
    public class GameData : MonoBehaviour
    {
        public const string DEPENDENCIES = "Dependencies";
        public const string SETTINGS = "Settings";

        #region Animator Hashes

        public static readonly int ATTACK_HASH = Animator.StringToHash("attack");
        public static readonly int COMBO_HASH = Animator.StringToHash("combo");
        public static readonly int ISMOVING_HASH = Animator.StringToHash("isMoving");


        #endregion

        #region Layers

        public static readonly LayerMask PLAYER_LAYER = LayerMask.GetMask("Player");
        public static readonly LayerMask TERRAIN_LAYER = LayerMask.GetMask("Terrain");
        public static readonly LayerMask ENEMY_LAYER = LayerMask.GetMask("Enemy");

        #endregion
    }
}