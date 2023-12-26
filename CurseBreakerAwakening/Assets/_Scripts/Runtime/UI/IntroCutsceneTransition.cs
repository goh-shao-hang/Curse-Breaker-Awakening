using CBA.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CBA
{
    public class IntroCutsceneTransition : MonoBehaviour
    {
        private void OnEnable()
        {
            GameManager.Instance.StartRun();
        }
    }
}