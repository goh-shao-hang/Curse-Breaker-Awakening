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
        [SerializeField] private CombatAnimationEventHander _weaponAnimationEventHander;

        private void Awake()
        {
            StopTrailEmission();
        }

        private void OnEnable()
        {
            _weaponAnimationEventHander.OnEmitTrail += StartTrailEmission;
            _weaponAnimationEventHander.OnStopEmitTrail += StopTrailEmission;
        }

        private void OnDisable()
        {
            _weaponAnimationEventHander.OnEmitTrail -= StartTrailEmission;
            _weaponAnimationEventHander.OnStopEmitTrail -= StopTrailEmission;
        }

        public void StartTrailEmission()
        {
            _trailRenderer.emitting = true;
        }

        public void StopTrailEmission()
        {
            _trailRenderer.emitting = false;
        }
    }
}