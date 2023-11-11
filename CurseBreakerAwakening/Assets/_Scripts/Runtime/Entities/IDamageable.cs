using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public interface IDamageable
    {
        void TakeDamage(float amount);
    }
}