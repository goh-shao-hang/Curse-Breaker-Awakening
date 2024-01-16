using GameCells.StateMachine;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class Enemy_ThrowTutorial : EnemyStateMachine
    {
        [SerializeField] private PhysicsQuery _playerDetector;
        [SerializeField] private PhysicsQuery _attackRangeDetector;
        [SerializeField] private GrabbableObject _grabbableObject;
        [SerializeField] private RagdollController _ragdollController;

        [Header("Attacks")]
        [SerializeField] private AttackData meleeAttack;

        [Header(GameData.CUSTOMIZATION)]
        [SerializeField] private float _chaseSpeed = 1f;
        [SerializeField] private float _meleeAttackDuration = 1f;
        [SerializeField] private float _engageDistance = 2f;
        [SerializeField] private float _engageDistanceBias = 1f;
        [SerializeField] private float _engagedMoveSpeed = 1f;
        [SerializeField] private float _engagedStrafeSpeed = 1f;

        #region States and Conditions
        private IdleState _idleState;
        private EngagedState _engagedState;
        private ChaseState _chaseState;
        private MeleeAttackState _meleeAttackState;
        private StunnedState _stunnedState;
        private GrabbedState _grabbedState;
        private DeathState _deathState;

        private Condition _playerInDetectionRangeCondition;
        private Condition _playerOutOfDetectionRangeCondition;
        private Condition _engagedTimerCondition;
        private Condition _playerInAttackRangeCondition;
        private Condition _meleeAttackTimerCondition;
        private Condition _guardBrokenCondition;
        private Condition _guardRecoverCondition;
        private Condition _grabbedCondition;
        private Condition _parriedCondition;
        private Condition _thrownTerrainCollisionCondition;
        private Condition _healthDepletedCondition;
        #endregion

        private void Awake()
        {
            //State Machine Initialization
            //1. State Initialization
            _idleState = new IdleState(entity, this);
            _engagedState = new EngagedState(entity, this, _engageDistance, _engageDistanceBias, _engagedMoveSpeed, _engagedStrafeSpeed);
            _chaseState = new ChaseState(entity, this, _chaseSpeed);
            _meleeAttackState = new MeleeAttackState(entity, this, meleeAttack);
            _stunnedState = new StunnedState(entity, this, _grabbableObject);
            _grabbedState = new GrabbedState(entity, this, _grabbableObject);
            _deathState = new DeathState(entity, this, _ragdollController);

            //2. Condition Initialization
            _playerInDetectionRangeCondition = new Condition_PlayerInRange(_playerDetector);
            _playerOutOfDetectionRangeCondition = new Condition_PlayerOutOfRange(_playerDetector);
            _engagedTimerCondition = new Condition_Timer_Random(3, 5);
            _playerInAttackRangeCondition = new Condition_PlayerInRange(_attackRangeDetector);
            _meleeAttackTimerCondition = new Condition_Timer(_meleeAttackDuration);
            _guardBrokenCondition = new Condition_GuardBroken(this.entity.GetModule<GuardModule>());
            _guardRecoverCondition = new Condition_GuardRecovered(this.entity.GetModule<GuardModule>());
            _grabbedCondition = new Condition_Grabbed(_grabbableObject);
            _parriedCondition = new Condition_Parried(this.GetComponent<EnemyHurtbox>());
            _thrownTerrainCollisionCondition = new Condition_ThrownTerrainCollision(_grabbableObject);
            _healthDepletedCondition = new Condition_HealthDepleted(this.entity.GetModule<HealthModule>());

            //3. Setting up transitions
            _idleState.AddTransition(_engagedState, _playerInDetectionRangeCondition);

            _engagedState.AddTransition(_idleState, _playerOutOfDetectionRangeCondition);
            _engagedState.AddTransition(_chaseState, _engagedTimerCondition);

            _chaseState.AddTransition(_meleeAttackState, _playerInAttackRangeCondition);

            _meleeAttackState.AddTransition(_engagedState, _meleeAttackTimerCondition);

            _stunnedState.AddTransition(_idleState, _guardRecoverCondition);
            _stunnedState.AddTransition(_grabbedState, _grabbedCondition);

            _grabbedState.AddTransition(_deathState, _thrownTerrainCollisionCondition);

            this.AddAnyTransition(_stunnedState, _guardBrokenCondition);

            Initialize(_idleState);
        }
    }
}