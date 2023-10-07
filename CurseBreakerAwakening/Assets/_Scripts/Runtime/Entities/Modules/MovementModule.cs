using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCells.Modules
{
    public class MovementModule : MonoBehaviour
    {
        [SerializeField] private Rigidbody _movementRigidbody;

        public Rigidbody MovementRigidbody => _movementRigidbody;

        public Vector3 CurrentVelocity => _movementRigidbody.velocity;


        public void SetDrag(float drag)
        {
            _movementRigidbody.drag = drag;
        }

        public void SetUseGravity(bool useGravity)
        {
            _movementRigidbody.useGravity = useGravity;
        }

        public void SetVelocity(Vector3 velocity)
        {
            _movementRigidbody.velocity = velocity;
        }

        public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Force)
        {
            _movementRigidbody.AddForce(force, forceMode);
        }
    }
}