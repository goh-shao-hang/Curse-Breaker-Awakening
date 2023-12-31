using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    [Serializable]
    public class AttackData
    {
        [field: SerializeField] public EntityWeapon Hitbox;
        [field: SerializeField] public float Damage;
    }
}