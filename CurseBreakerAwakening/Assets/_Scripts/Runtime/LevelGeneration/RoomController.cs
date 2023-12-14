using CBA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.LevelGeneration
{
    public class RoomController : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private GameObject[] _walls;
        [SerializeField] private GameObject[] _doors;

        public void UpdateExits(bool[] hasExit)
        {
            for (int i = 0; i < hasExit.Length; i++)
            {
                _doors[i].SetActive(hasExit[i]);
                _walls[i].SetActive(!hasExit[i]);
            }
        }
    }
}