using CBA;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCheck : PhysicsQuery
{
    [Header(GameData.SETTINGS)]
    [SerializeField] private Vector3 _raycastDirection;
    [SerializeField] private float _maxDistance;

    private RaycastHit _hitInfo;
    public RaycastHit HitInfo => _hitInfo;

    public override bool Hit()
    {
        return Physics.Raycast(transform.position + _offset, _raycastDirection, out _hitInfo, _maxDistance, _targetLayers);
    }

    public override void OnVisualize()
    {
        Gizmos.DrawLine(transform.position + _offset, transform.position + _offset + _raycastDirection * _maxDistance);
    }
}
