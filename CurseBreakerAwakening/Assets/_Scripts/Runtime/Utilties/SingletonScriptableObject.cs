using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCells.Utilities
{
    public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] assets = Resources.LoadAll<T>("");
                    if (assets == null || assets.Length < 1)
                    {
                        throw new System.Exception($"No SingletonScriptableObject of type {nameof(T)} found in Resources folder!");
                    }
                    else if (assets.Length > 1)
                    {
                        Debug.LogWarning($"More than 1 instance of SingletonScriptableObject of type {nameof(T)} was found in Resources folder!");
                    }

                    _instance = assets[0];
                }

                return _instance;
            }
        }
    }
}