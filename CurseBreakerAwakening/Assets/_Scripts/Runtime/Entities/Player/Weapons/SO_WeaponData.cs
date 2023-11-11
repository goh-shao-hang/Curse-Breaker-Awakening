using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player.Weapons
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon Data")]
    public class SO_WeaponData : ScriptableObject
    {
        public string Name;
        public ERarity Rarity;
        public EWeaponType Type;
        public Weapon WeaponPrefab;

        public int MaxCombo = 3;
        public float ComboResetTime = 1.5f;

    }
}