using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private Transform _cameraFollowTarget;

        private void Update()
        {
            transform.position = _cameraFollowTarget.transform.position;
        }
    }
}