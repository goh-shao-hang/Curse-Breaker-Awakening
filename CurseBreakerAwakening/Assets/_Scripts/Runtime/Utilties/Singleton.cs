using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCells.Utilities
{
    [DefaultExecutionOrder(-100)]
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                    return null;//instance = FindObjectOfType<T>(); //new GameObject(typeof(T).ToString()).AddComponent<T>();
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void OnApplicationQuit()
        {
            RemoveInstance();
        }

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RemoveInstance()
        {
            instance = null;
        }

        protected void SetDontDestroyOnLoad(bool dontDestroyOnLoad)
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
            }
        }
    }
}