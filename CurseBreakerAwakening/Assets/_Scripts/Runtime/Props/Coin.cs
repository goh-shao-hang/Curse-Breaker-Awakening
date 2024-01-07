using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA
{
    public class Coin : Loot
    {
        protected override void OnCollected(GameObject _playerGameObject)
        {
            InGameResourceManager.Instance?.ObtainCoin(1);
        }
    }
}