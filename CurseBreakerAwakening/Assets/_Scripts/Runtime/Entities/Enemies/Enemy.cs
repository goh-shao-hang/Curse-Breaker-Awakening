using CBA;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class Enemy : Entity
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Animator _animator;
        [SerializeField] private RagdollController _ragdollController;

        protected override void Die()
        {
            base.Die();

            _ragdollController.EnableRagdoll();
            Destroy(gameObject, 3f);
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);

            _animator.SetTrigger(GameData.HIT_HASH);
        }
    }
}