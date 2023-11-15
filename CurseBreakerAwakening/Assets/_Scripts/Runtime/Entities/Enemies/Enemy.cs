using CBA;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class Enemy : Entity
    {
        [SerializeField] private Animator _animator;

        protected override void Die()
        {
            base.Die();

            Destroy(gameObject);
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);

            _animator.SetTrigger(GameData.HIT_HASH);
        }
    }
}