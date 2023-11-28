using CBA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrabbableObject : MonoBehaviour, IInteractable
{
    [Header(GameData.DEPENDENCIES)]
    [SerializeField] private Rigidbody _grabRigidbody;

    [Header(GameData.SETTINGS)]
    [SerializeField] private bool _startGrabbable = true;
    [SerializeField] private float _thrownForce = 5f;
    [SerializeField] private float _grabSmoothing = 10f;

    public bool IsGrabbable { get; private set; }

    private Transform _grabberTransform;
    private Transform _grabTransform;

    public UnityEvent OnStartHighlight;
    public UnityEvent OnStopHighlight;
    public UnityEvent OnGrabbed;
    public UnityEvent OnReleased;

    private void Start()
    {
        //_grabRigidbody.isKinematic = true;

        _grabRigidbody.isKinematic = true;
        _grabRigidbody.useGravity = false;
        _grabRigidbody.detectCollisions = true;

        IsGrabbable = _startGrabbable;
    }

    private void Update()
    {
        if (_grabTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, _grabTransform.position, Time.deltaTime * _grabSmoothing);
            transform.LookAt(_grabberTransform);
        }
    }

    public void StartGrabbing(Transform grabTransform, Transform _grabberTransform)
    {
        this._grabTransform = grabTransform;
        this._grabberTransform = _grabberTransform;

        _grabRigidbody.useGravity = false;
        _grabRigidbody.detectCollisions = false;

        OnGrabbed?.Invoke();
    }

    public void StopGrabbing()
    {
        this._grabTransform = null;
        this._grabberTransform = null;

        _grabRigidbody.isKinematic = true;
        _grabRigidbody.useGravity = false;
        _grabRigidbody.detectCollisions = true;

        OnReleased?.Invoke();
    }

    public void Throw(Vector3 direction)
    {
        this._grabTransform = null;

        _grabRigidbody.isKinematic = false;
        _grabRigidbody.useGravity = true;
        _grabRigidbody.detectCollisions = true;

        _grabRigidbody.AddForce(direction * _thrownForce, ForceMode.Impulse);
    }

    public void OnSelect()
    {
        OnStartHighlight?.Invoke();
    }

    public void OnDeselect()
    {
        OnStopHighlight?.Invoke();
    }

    public void OnInteract()
    {
    }
}
