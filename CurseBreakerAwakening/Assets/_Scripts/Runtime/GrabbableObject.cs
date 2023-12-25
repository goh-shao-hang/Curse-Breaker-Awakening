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
    [SerializeField] private Vector3 _grabbedOffset;

    public bool IsGrabbable { get; private set; }

    private Transform _grabTransform;

    public UnityEvent<bool> OnGrabbableStateChanged;
    public UnityEvent OnStartHighlight;
    public UnityEvent OnStopHighlight;
    [HideInInspector] public UnityEvent OnGrabbed;
    [HideInInspector] public UnityEvent OnThrown;
    [HideInInspector] public UnityEvent OnTerrainCollision;

    private bool _thrown = false;

    private Transform _originalParent;

    private void Start()
    {
        EnableThrowPhysics(false);

        IsGrabbable = _startGrabbable;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_thrown)
            return;

        if (collision.collider.gameObject.layer == GameData.TERRAIN_LAYER_INDEX)
        {
            OnTerrainCollision?.Invoke();
            _thrown = false;
        }
    }

    public void SetIsGrabbable(bool isGrabbable)
    {
        IsGrabbable = isGrabbable;
        OnGrabbableStateChanged.Invoke(isGrabbable);

        /*if (!isGrabbable)
        {
            _grabRigidbody.isKinematic = true;
        }*/
    }

    public void EnableThrowPhysics(bool enabled)
    {
        _grabRigidbody.isKinematic = !enabled;
    }

    public void StartGrabbing(Transform grabTransform, Transform _grabberTransform)
    {
        this._grabTransform = grabTransform;

        _originalParent = transform.parent;

        transform.SetParent(_grabTransform);
        transform.SetLocalPositionAndRotation(_grabbedOffset, Quaternion.Euler(0f, 180f, 0f));

        EnableThrowPhysics(false);

        //TODO use some other way
        _grabRigidbody.detectCollisions = false; //Prevent attacking this object or colliding with other objects

        OnGrabbed?.Invoke();
    }

    public void Throw(Vector3 direction, Vector3 carriedVelocity)
    {
        this._grabTransform = null;

        transform.SetParent(_originalParent == null ? null : _originalParent);

        EnableThrowPhysics(true);

        //TODO use some other way
        _grabRigidbody.detectCollisions = true;

        _grabRigidbody.velocity = carriedVelocity; //Inherit velocity from player

        _grabRigidbody.AddForce(direction * _thrownForce, ForceMode.Impulse);

        OnThrown?.Invoke();

        _thrown = true;
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
        throw new System.NotImplementedException();
    }
}
