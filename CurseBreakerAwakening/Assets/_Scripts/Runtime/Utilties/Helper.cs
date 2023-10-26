using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCells.Utilities
{
    public static class Helper
    {

        private static Camera _mainCamera = null;
        public static Camera MainCamera => _mainCamera ??= Camera.main;


        /// <summary>
        /// Return true or false based on a probability between 0 and 1.
        /// </summary>
        /// <param name="successChance"></param>
        /// <returns></returns>
        public static bool SuccessRateCheck(float successChance)
        {
            float random = Random.value;
            if (successChance >= random)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}