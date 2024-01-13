using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public interface IDamageable
    {
        void TakeDamage(DamageData damageData);
    }

    public struct DamageData
    {
        public float DamageAmount;
        public GameObject Attacker;
        public bool CanBeBlocked;
        public bool IsGuardDamage;

        public DamageData(float damageAmount, GameObject attacker, bool canBeBlocked = true, bool isGuardDamage = false)
        {
            DamageAmount = damageAmount;
            Attacker = attacker;
            CanBeBlocked = canBeBlocked;
            IsGuardDamage = isGuardDamage;
        }

        public void Set(float damageAmount, GameObject attacker, bool canBeBlocked = true, bool isGuardDamage = false)
        {
            DamageAmount = damageAmount;
            Attacker = attacker;
            CanBeBlocked = canBeBlocked;
            IsGuardDamage = isGuardDamage;
        }
    }
}