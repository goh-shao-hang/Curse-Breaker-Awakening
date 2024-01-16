using CBA.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA
{
    public class Coin : Loot
    {
        public override Type GetLootType()
        {
            return this.GetType();
        }

        protected override void OnCollected(GameObject _playerGameObject)
        {
            base.OnCollected(_playerGameObject);

            GameManager.Instance?.ObtainCoin(1);
        }
    }
}