using GameCells.StateMachine;
using GameCells.Utilities;
using UnityEngine;

namespace CBA.Entities
{
    public class Enemy_ExplosiveBeetle : EnemyStateMachine
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private PhysicsQuery _playerDetector;
        [SerializeField] private PhysicsQuery _explodeRangeDetector;
        [SerializeField] private GrabbableObject _grabbableObject;

        [Header(GameData.CUSTOMIZATION)]
        [SerializeField] private float _chaseSpeed = 1f;
        [SerializeField] private float _explosionTimer = 3f;

        #region States and Conditions
        private IdleState _idleState;
        private ChaseState _chaseState;
        private GrabbedState _grabbedState;
        private DeathState _deathState;
        private PrepareExplodeState _prepareExplodeState;
        private ExplodeState _explodeState;

        private Condition _playerInDetectionRangeCondition;
        private Condition _playerOutOfDetectionRangeCondition;
        private Condition _playerInExplosionRangeCondition;
        private Condition _explosionReadyCondition;
        private Condition _grabbedCondition;
        private Condition _thrownTerrainCollisionCondition;
        private Condition _healthDepletedCondition;
        #endregion

        private void Awake()
        {
            //State Machine Initialization
            //1. State Initialization
            _idleState = new IdleState(entity, this);
            _chaseState = new ChaseState(entity, this, _chaseSpeed);
            _grabbedState = new GrabbedState(entity, this, _grabbableObject);
            _deathState = new DeathState(entity, this, null, this._grabbableObject);
            _prepareExplodeState = new PrepareExplodeState(entity, this, _explosionTimer);
            _explodeState = new ExplodeState(entity, this);

            //2. Condition Initialization
            _playerInDetectionRangeCondition = new Condition_PlayerInRange(_playerDetector);
            _playerOutOfDetectionRangeCondition = new Condition_PlayerOutOfRange(_playerDetector);
            _playerInExplosionRangeCondition = new Condition_PlayerInRange(_explodeRangeDetector);
            _explosionReadyCondition = new Condition_Timer(_explosionTimer);
            _grabbedCondition = new Condition_Grabbed(_grabbableObject);
            _thrownTerrainCollisionCondition = new Condition_ThrownTerrainCollision(_grabbableObject);
            _healthDepletedCondition = new Condition_HealthDepleted(ModuleManager.GetModule<HealthModule>());

            //3. Setting up transitions
            _idleState.AddTransition(_chaseState, _playerInDetectionRangeCondition);

            _chaseState.AddTransition(_idleState, _playerOutOfDetectionRangeCondition);
            _chaseState.AddTransition(_prepareExplodeState, _playerInExplosionRangeCondition);

            _prepareExplodeState.AddTransition(_explodeState, _explosionReadyCondition);

            _grabbedState.AddTransition(_explodeState, _thrownTerrainCollisionCondition);

            this.AddAnyTransition(_grabbedState, _grabbedCondition);
            this.AddAnyTransition(_deathState, _healthDepletedCondition);

            Initialize(_idleState);
        }

        protected override void Update()
        {
            base.Update();

            //Debug.LogError(_currentState);
        }
    }
}