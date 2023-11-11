using CBA;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class Enemy : Entity
    {
        protected override void Die()
        {
            base.Die();

            Destroy(gameObject);
        }
    }
}