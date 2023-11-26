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
        public float AttackGuardDamage = 3f;

        [Header("Charged Attack")]
        public float MinChargedAttackDamage = 5f;
        public float MaxChargedAttackDamage = 10f;
        public float MinChargedAttackGuardDamage = 5f;
        public float MaxChargedAttackGuardDamage = 10f;

        [Header("Combo")]
        public int MaxCombo = 3;
        //TODO unused
        public float ComboResetTime = 1.5f;
    }
}