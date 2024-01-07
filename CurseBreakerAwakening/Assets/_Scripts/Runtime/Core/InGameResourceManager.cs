using CBA.Entities;
using GameCells.Utilities;
using UnityEngine;

public class InGameResourceManager : Singleton<InGameResourceManager>
{
    public int Coins { get; private set; } = 0;

    public void ObtainCoin(int amount)
    {
        Coins += amount;
        Debug.Log($"You have {Coins} coins.");
    }

    public void ObtainPotion()
    {

    }
}
