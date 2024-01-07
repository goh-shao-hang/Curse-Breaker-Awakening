using CBA;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CBA.Entities;
using CBA.Entities.Player;

public class GrabbableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private Rigidbody _grabRigidbody;

    [Header(GameData.SETTINGS)]
    [Tooltip("Should this object interact with environment upon game start?")]
    [SerializeField] private bool _startKinematic = true;
    [SerializeField] private bool _startGrabbable = true;
    [SerializeField] private float _thrownForce = 10f;
    [SerializeField] private float _thrownSelfDamage = 5f;
    [SerializeField] private float _thrownInflictedDamage = 5f;
    [SerializeField] private Vector3 _grabbedOffset;
    [SerializeField] private LayerMask _canCollideWith;
    [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderers;

    public bool IsGrabbable { get; private set; }

    private Transform _grabTransform;
    private int _originalLayerIndex;
    private List<int> _meshOriginalLayerIndices = new List<int>();

    public UnityEvent<bool> OnGrabbableStateChanged;
    public UnityEvent OnStartHighlight;
    public UnityEvent OnStopHighlight;
    [HideInInspector] public UnityEvent OnGrabbed;
    [HideInInspector] public UnityEvent OnThrown;
    [HideInInspector] public UnityEvent OnThrowCollision;

    private bool _thrown = false;

    private Transform _originalParent;

    private void Start()
    {
        EnableThrowPhysics(!_startKinematic);

        SetIsGrabbable(_startGrabbable);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_thrown)
            return;

        if ((_canCollideWith & (1 << collision.gameObject.layer)) != 0)
        {
            OnThrowCollision?.Invoke();

            GetComponent<IDamageable>()?.TakeDamage(_thrownSelfDamage);
            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(_thrownInflictedDamage);

            _thrown = false;
        }
    }

    public void SetIsGrabbable(bool isGrabbable)
    {
        IsGrabbable = isGrabbable;
        OnGrabbableStateChanged.Invoke(isGrabbable);
    }

    public void EnableThrowPhysics(bool enabled)
    {
        _grabRigidbody.isKinematic = !enabled;
    }

    public void OnInteract(PlayerGrabManager playerGrabManager)
    {
        if (!IsGrabbable)
            return;

        playerGrabManager.Grab(this);
    }

    public void StartGrabbing(Transform grabTransform, Transform _grabberTransform)
    {
        this._grabTransform = grabTransform;

        _originalLayerIndex = this.gameObject.layer;
        this.gameObject.layer = GameData.WEAPON_LAYER_INDEX;

        for (int i = 0; i < _skinnedMeshRenderers.Length; i++)
        {
            _meshOriginalLayerIndices.Add(_skinnedMeshRenderers[i].gameObject.layer);
            _skinnedMeshRenderers[i].gameObject.layer = GameData.WEAPON_LAYER_INDEX;
        }

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

        this.gameObject.layer = _originalLayerIndex;

        for (int i = 0; i < _skinnedMeshRenderers.Length; i++)
        {
            _skinnedMeshRenderers[i].gameObject.layer = _meshOriginalLayerIndices[i];
        }

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
        if (!IsGrabbable)
            return;

        OnStartHighlight?.Invoke();
    }

    public void OnDeselect()
    {
        OnStopHighlight?.Invoke();
    }


}
