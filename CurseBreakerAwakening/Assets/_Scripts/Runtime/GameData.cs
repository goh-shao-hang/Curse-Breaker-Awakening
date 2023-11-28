using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA
{
    public class GameData : MonoBehaviour
    {
        public const string DEPENDENCIES = "Dependencies";
        public const string SETTINGS = "Settings";
        public const string CUSTOMIZATION = "Cutomization";

        #region Animator Hashes

        public static readonly int ATTACK_HASH = Animator.StringToHash("attack");
        public static readonly int COMBO_HASH = Animator.StringToHash("combo");
        public static readonly int ISMOVING_HASH = Animator.StringToHash("isMoving");
        public static readonly int ISCHARGING_HASH = Animator.StringToHash("isCharging");
        public static readonly int FULLYCHARGED_HASH = Animator.StringToHash("fullyCharged");
        public static readonly int CHARGEDATTACKENDED_HASH = Animator.StringToHash("chargedAttackEnded");
        public static readonly int HIT_HASH = Animator.StringToHash("hit");
        public static readonly int XVELOCITY_HASH = Animator.StringToHash("xVelocity");
        public static readonly int ZVELOCITY_HASH = Animator.StringToHash("zVelocity");
        public static readonly int ISSTUNNED_HASH = Animator.StringToHash("isStunned");
        public static readonly int ISGRABBED_HASH = Animator.StringToHash("isGrabbed");

        #endregion

        #region Layers

        public static readonly LayerMask PLAYER_LAYER = LayerMask.GetMask("Player");
        public static readonly LayerMask TERRAIN_LAYER = LayerMask.GetMask("Terrain");
        public static readonly LayerMask DAMAGEABLE_LAYER = LayerMask.GetMask("Damageable");

        public static readonly int PLAYER_LAYER_INDEX = LayerMask.NameToLayer("Player");
        public static readonly int DAMAGEABLE_LAYER_INDEX = LayerMask.NameToLayer("Damageable");

        #endregion
    }
}