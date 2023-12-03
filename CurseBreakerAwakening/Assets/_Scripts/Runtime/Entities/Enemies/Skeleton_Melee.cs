using CBA.Entities.Player.Weapons;
using GameCells.StateMachine;
using GameCells.Utilities;
using UnityEngine;

namespace CBA.Entities
{
    public class Skeleton_Melee : EnemyStateMachine
    {
        [SerializeField] private PhysicsQuery _playerDetector;
        [SerializeField] private PhysicsQuery _attackRangeDetector;
        [SerializeField] private GrabbableObject _grabbableObject;
        [SerializeField] private RagdollController _ragdollController;

        [Header("Attacks")]
        [SerializeField] private AttackData meleeAttack;

        [Header(GameData.CUSTOMIZATION)]
        [SerializeField] private float _meleeAttackDuration = 1f;

        #region States and Conditions
        private IdleState _idleState;
        private ChaseState _chaseState;
        private MeleeAttackState _meleeAttackState;
        private StunnedState _stunnedState;
        private GrabbedState _grabbedState;
        private RecoverState _recoverState;
        private DeathState _deathState;

        private Condition _playerInDetectionRangeCondition;
        private Condition _playerOutOfDetectionRangeCondition;
        private Condition _playerInAttackRangeCondition;
        private Condition _meleeAttackTimerCondition;
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
            _idleState = new IdleState(_entity, this);
            _chaseState = new ChaseState(_entity, this);
            _meleeAttackState = new MeleeAttackState(_entity, this, meleeAttack);
            _stunnedState = new StunnedState(_entity, this, _grabbableObject);
            _grabbedState = new GrabbedState(_entity, this, _grabbableObject);
            _recoverState = new RecoverState(_entity, this);
            _deathState = new DeathState(_entity, this, _ragdollController);

            //2. Condition Initialization
            _playerInDetectionRangeCondition = new Condition_PlayerInRange(_playerDetector);
            _playerOutOfDetectionRangeCondition = new Condition_PlayerOutOfRange(_playerDetector);
            _playerInAttackRangeCondition = new Condition_PlayerInRange(_attackRangeDetector);
            _meleeAttackTimerCondition = new Condition_Timer(_meleeAttackDuration);
            _guardBrokenCondition = new Condition_GuardBroken(GuardModule);
            _stunTimerCondition = new Condition_Timer(_entity.EntityData.BaseStunDuration);
            _grabbedCondition = new Condition_Grabbed(_grabbableObject);
            _thrownTerrainCollisionCondition = new Condition_ThrownTerrainCollision(_grabbableObject);
            _recoverAnimationFinishedCondition = new Condition_OnAnimationFinished(Animator);
            _healthDepletedCondition = new Condition_HealthDepleted(HealthModule);

            //3. Setting up transitions
            _idleState.AddTransition(_chaseState, _playerInDetectionRangeCondition);

            _chaseState.AddTransition(_idleState, _playerOutOfDetectionRangeCondition);
            _chaseState.AddTransition(_meleeAttackState, _playerInAttackRangeCondition);

            _meleeAttackState.AddTransition(_chaseState, _meleeAttackTimerCondition);

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