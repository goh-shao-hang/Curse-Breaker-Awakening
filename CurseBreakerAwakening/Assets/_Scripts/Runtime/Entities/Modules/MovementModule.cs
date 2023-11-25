using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCells.Entities
{
    public class MovementModule : MonoBehaviour
    {
        [SerializeField] private Rigidbody _movementRigidbody;

        public Rigidbody MovementRigidbody => _movementRigidbody;

        public Vector3 CurrentVelocity => _movementRigidbody.velocity;

        
        private Vector3 _velocityCache; //Reused to reduce garbage collection

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

        public void SetVelocity(float xVelocity, float yVelocity, float zVelocity)
        {
            _velocityCache.Set(xVelocity, yVelocity, zVelocity);
            _movementRigidbody.velocity = _velocityCache;
        }

        public void SetXVelocity(float xVelocity)
        {
            _velocityCache.Set(xVelocity, CurrentVelocity.y, CurrentVelocity.z);
            _movementRigidbody.velocity = _velocityCache;
        }

        public void SetYVelocity(float yVelocity)
        {
            _velocityCache.Set(CurrentVelocity.x, yVelocity, CurrentVelocity.z);
            _movementRigidbody.velocity = _velocityCache;
        }

        public void SetZVelocity(float zVelocity)
        {
            _velocityCache.Set(CurrentVelocity.x, CurrentVelocity.y, zVelocity);
            _movementRigidbody.velocity = _velocityCache;
        }

        public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Force)
        {
            _movementRigidbody.AddForce(force, forceMode);
        }
    }
}