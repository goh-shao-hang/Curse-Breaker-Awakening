using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCheck : PhysicsQuery
{
    [Header(GameData.SETTINGS)]
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _radius;

    public override bool Hit()
    {
        return Physics.CheckSphere(transform.position + _offset, _radius, _targetLayers);
    }

    public override void OnVisualize()
    {
        Gizmos.DrawWireSphere(transform.position + _offset, _radius);
    }
}
