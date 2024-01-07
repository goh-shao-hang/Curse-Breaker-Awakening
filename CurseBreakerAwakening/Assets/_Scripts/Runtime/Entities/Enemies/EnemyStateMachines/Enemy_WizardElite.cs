using GameCells.StateMachine;
using GameCells.Utilities;
using UnityEngine;

namespace CBA.Entities
{
    public class Enemy_WizardElite : EnemyStateMachine
    {
        [SerializeField] private PhysicsQuery _playerDetector;
        [SerializeField] private PhysicsQuery _attackRangeDetector;
        [SerializeField] private GrabbableObject _grabbableObject;
        [SerializeField] private RagdollController _ragdollController;

        [Header("Attacks")]
        [SerializeField] private ProjectileShooter _rangedAttack;

        [Header(GameData.CUSTOMIZATION)]
        [SerializeField] private float _chaseSpeed = 1f;
        [SerializeField] private float _retreatSpeed = 1f;
        [SerializeField] private float _attackDuration = 1f;
        [SerializeField] private float _attackDelay = 0.5f;
        [SerializeField] private float _retreatTime = 2f;

        #region States and Conditions
        private IdleState _idleState;
        private ChaseState _chaseState;
        private RangedAttackState _rangedAttackState;
        private RetreatState _retreatState;
        private StunnedState _stunnedState;
        private GrabbedState _grabbedState;
        private RecoverState _recoverState;
        private DeathState _deathState;

        private Condition _playerInDetectionRangeCondition;
        private Condition _playerOutOfDetectionRangeCondition;
        private Condition _playerInAttackRangeCondition;
        private Condition _rangedAttackTimerCondition;
        private Condition _retreatTimerCondition;
        private Condition _guardBrokenCondition;
        private Condition _stunTimerCondition;
        private Condition _grabbedCondition;
        private Condition _thrownTerrainCollisionCondition;
        private Condition _recoverAnimationFinishedCondition;
        private Condition _healthDepletedCondition;
        #endregion

        private void Awake()
        {
            //State Machine Initialization
            //1. State Initialization
            _idleState = new IdleState(entity, this);
            _chaseState = new ChaseState(entity, this, _chaseSpeed);
            _rangedAttackState = new RangedAttackState(entity, this, _rangedAttack, _attackDelay);
            _retreatState = new RetreatState(entity, this, _retreatSpeed);
            _stunnedState = new StunnedState(entity, this, _grabbableObject);
            _grabbedState = new GrabbedState(entity, this, _grabbableObject);
            _recoverState = new RecoverState(entity, this, _grabbableObject);
            _deathState = new DeathState(entity, this, _ragdollController);

            //2. Condition Initialization
            _playerInDetectionRangeCondition = new Condition_PlayerInRange(_playerDetector);
            _playerOutOfDetectionRangeCondition = new Condition_PlayerOutOfRange(_playerDetector);
            _playerInAttackRangeCondition = new Condition_PlayerInRange(_attackRangeDetector);
            _rangedAttackTimerCondition = new Condition_Timer(_attackDuration);
            _retreatTimerCondition = new Condition_Timer(_retreatTime);
            _guardBrokenCondition = new Condition_GuardBroken(this.ModuleManager.GetModule<GuardModule>());
            _stunTimerCondition = new Condition_Timer(entity.EntityData.BaseStunDuration);
            _grabbedCondition = new Condition_Grabbed(_grabbableObject);
            _thrownTerrainCollisionCondition = new Condition_ThrownTerrainCollision(_grabbableObject);
            _recoverAnimationFinishedCondition = new Condition_OnAnimationFinished(Animator);
            _healthDepletedCondition = new Condition_HealthDepleted(this.ModuleManager.GetModule<HealthModule>());

            //3. Setting up transitions
            _idleState.AddTransition(_chaseState, _playerInDetectionRangeCondition);

            _chaseState.AddTransition(_idleState, _playerOutOfDetectionRangeCondition);
            _chaseState.AddTransition(_rangedAttackState, _playerInAttackRangeCondition);

            _rangedAttackState.AddTransition(_retreatState, _rangedAttackTimerCondition);

            _retreatState.AddTransition(_chaseState, _retreatTimerCondition);

            _stunnedState.AddTransition(_idleState, _stunTimerCondition);
            _stunnedState.AddTransition(_grabbedState, _grabbedCondition);

            _grabbedState.AddTransition(_recoverState, _thrownTerrainCollisionCondition);

            _recoverState.AddTransition(_idleState, _recoverAnimationFinishedCondition);

            this.AddAnyTransition(_stunnedState, _guardBrokenCondition);
            this.AddAnyTransition(_deathState, _healthDepletedCondition);

            Initialize(_idleState);
        }
    }
}