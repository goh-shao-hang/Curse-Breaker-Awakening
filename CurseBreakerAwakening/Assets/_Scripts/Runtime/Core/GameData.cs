using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA
{
    public class GameData : MonoBehaviour
    {
        public const string DEPENDENCIES = "Dependencies";
        public const string SETTINGS = "Settings";
        public const string DEBUG = "Debug";
        public const string CUSTOMIZATION = "Cutomization";
        public const string MODULES = "Modules";

        #region Hidden Variables

        public const float LEVEL_TRANSITION_TIME = 5f;
        public const float SCENE_TRANSITION_DURATION = 2f;
        public const float PROPS_DESTROYED_DELAY = 5f;

        #endregion

        #region Scenes

        public const string MAINMENU_SCENE = "MainMenu";
        public const string INTRO_SCENE = "Intro";
        public const string GENERATION_SCENE = "Generation";

        #endregion

        #region Animator Hashes and Strings

        public const float ANIMATIONDAMPTIME = 0.2f;

        public static readonly int ATTACK_HASH = Animator.StringToHash("attack");
        public static readonly int RANGEDATTACK_HASH = Animator.StringToHash("rangedAttack");
        public static readonly int COMBO_HASH = Animator.StringToHash("combo");
        public static readonly int ISMOVING_HASH = Animator.StringToHash("isMoving");
        public static readonly int ISBlOCKING_HASH = Animator.StringToHash("isBlocking");
        public static readonly int BLOCKSUCCESS_HASH = Animator.StringToHash("blockSuccess");
        public static readonly int PARRY_HASH = Animator.StringToHash("parry");
        public static readonly int ISCHARGING_HASH = Animator.StringToHash("isCharging");
        public static readonly int FULLYCHARGED_HASH = Animator.StringToHash("fullyCharged");
        public static readonly int CHARGERELEASED_HASH = Animator.StringToHash("chargeReleased");
        public static readonly int CHARGEDATTACKENDED_HASH = Animator.StringToHash("chargedAttackEnded");
        public static readonly int HIT_HASH = Animator.StringToHash("hit");
        public static readonly int DIE_HASH = Animator.StringToHash("die");
        public static readonly int PREPARINGEXPLODE_HASH = Animator.StringToHash("preparingExplode");
        public static readonly int SPEED_HASH = Animator.StringToHash("speed");
        public static readonly int XVELOCITY_HASH = Animator.StringToHash("xVelocity");
        public static readonly int ZVELOCITY_HASH = Animator.StringToHash("zVelocity");
        public static readonly int ISSTUNNED_HASH = Animator.StringToHash("isStunned");
        public static readonly int ISGRABBED_HASH = Animator.StringToHash("isGrabbed");

        //BOSS
        public static readonly int ACTIVATESHIELD_HASH = Animator.StringToHash("activateShield");

        public const string LOCOMOTION_ANIM = "Locomotion";
        public const string ATTACK_ANIM = "Attack";
        public const string STUNNED_ANIM = "Stunned";
        public const string GRABBED_ANIM = "Grabbed";
        public const string RECOVER_ANIM = "Recover";
        public const string HIT_ANIM = "Hit";

        public enum EEnemyAnimatorLayers
        {
            Default = 0,
            Damage = 1
        }

        #endregion

        #region Material Parameters
        public const float DAMAGE_EFFECT_DURATION = 0.5f;

        public const string DAMAGE_STRENGTH = "_DamageStrength";
        public const string GLOW_STRENGTH = "_GlowStrength";

        public const string DISSOLVE = "_Dissolve";
        #endregion

        #region Layers

        public static readonly LayerMask PLAYER_LAYER = LayerMask.GetMask("Player");
        public static readonly LayerMask TERRAIN_LAYER = LayerMask.GetMask("Terrain");
        public static readonly LayerMask DAMAGEABLE_LAYER = LayerMask.GetMask("Damageable");
        public static readonly LayerMask WEAPON_LAYER = LayerMask.GetMask("Weapon");

        public static readonly int PLAYER_LAYER_INDEX = LayerMask.NameToLayer("Player");
        public static readonly int TERRAIN_LAYER_INDEX = LayerMask.NameToLayer("Terrain");
        public static readonly int DAMAGEABLE_LAYER_INDEX = LayerMask.NameToLayer("Damageable");
        public static readonly int WEAPON_LAYER_INDEX = LayerMask.NameToLayer("Weapon");

        #endregion
    }
}