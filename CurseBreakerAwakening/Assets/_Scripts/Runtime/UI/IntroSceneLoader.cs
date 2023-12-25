using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CBA
{
    public class IntroSceneLoader : MonoBehaviour
    {
        public SceneField SceneField;

        private void OnEnable()
        {
            if (SceneField != null)
            {
                SceneManager.LoadScene(SceneField);
            }
        }
    }
}