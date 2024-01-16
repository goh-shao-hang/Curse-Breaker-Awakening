using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player.Weapons
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon Data")]
    public class SO_WeaponData : ScriptableObject
    {
        [Header("Basic Info")]
        public string Name;
        public ERarity Rarity;
        public EWeaponType Type;
        public Weapon WeaponPrefab;

        [Header("Basic Attack")]
        public float AttackDamage = 3f;

        [Header("Charged Attack")]
        public float MinChargingTime = 0.5f;
        public float MaxChargingTime = 2f;
        public float ChargedAttackDuration = 0.5f;
        public float MinChargedAttackDamage = 5f;
        public float MaxChargedAttackDamage = 10f;

        [Header("Block")]
        public float ParryDuration = 0.2f;
        public float MinBlockTime = 0.1f;
        public float ParryGuardDamage = 8f;

        [Header("Combo")]
        public int MaxCombo = 3;
        public float ComboResetTime = 1.5f; //Unused
    }
}