using CBA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player.Weapons
{
    public class WeaponTrail : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private TrailRenderer _trailRenderer;
        [SerializeField] private WeaponAnimationEventHander _weaponAnimationEventHander;

        private void Awake()
        {
            StopTrailEmission();
        }

        private void OnEnable()
        {
            _weaponAnimationEventHander.OnActivateHitboxEvent += StartTrailEmission;
            _weaponAnimationEventHander.OnDeactivateHitboxEvent += StopTrailEmission;
        }

        private void OnDisable()
        {
            _weaponAnimationEventHander.OnActivateHitboxEvent -= StartTrailEmission;
            _weaponAnimationEventHander.OnDeactivateHitboxEvent -= StopTrailEmission;
        }

        private void StartTrailEmission()
        {
            _trailRenderer.emitting = true;
        }

        private void StopTrailEmission()
        {
            _trailRenderer.emitting = false;
        }
    }
}