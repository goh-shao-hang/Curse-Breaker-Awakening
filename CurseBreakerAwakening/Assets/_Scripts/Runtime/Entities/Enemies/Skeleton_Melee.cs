using GameCells.StateMachine;
using GameCells.Utilities;
using UnityEngine;

namespace CBA.Entities
{
    public class Skeleton_Melee : StateMachine
    {
        [SerializeField] private Entity _entity;
        [SerializeField] private PhysicsQuery _playerDetector;
        [SerializeField] private PhysicsQuery _attackRangeDetector;

        [Header(GameData.CUSTOMIZATION)]
        [SerializeField] private float _meleeAttackDuration = 1f;

        #region States and Conditions
        private IdleState _idleState;
        private ChaseState _chaseState;
        private MeleeAttackState _meleeAttackState;

        private Condition _playerInDetectionRangeCondition;
        private Condition _playerOutOfDetectionRangeCondition;
        private Condition _playerInAttackRangeCondition;
        private Condition _meleeAttackTimerCondition;
        #endregion

        private void Awake()
        {
            //State Machine Initialization
            _idleState = new IdleState(_entity);
            _chaseState = new ChaseState(_entity);
            _meleeAttackState = new MeleeAttackState(_entity);

            _playerInDetectionRangeCondition = new Condition_PlayerInRange(_playerDetector);
            _playerOutOfDetectionRangeCondition = new Condition_PlayerOutOfRange(_playerDetector);
            _playerInAttackRangeCondition = new Condition_PlayerInRange(_attackRangeDetector);
            _meleeAttackTimerCondition = new Condition_Timer(_meleeAttackDuration);

            _idleState.AddTransition(_chaseState, _playerInDetectionRangeCondition);

            _chaseState.AddTransition(_idleState, _playerOutOfDetectionRangeCondition);
            _chaseState.AddTransition(_meleeAttackState, _playerInAttackRangeCondition);

            _meleeAttackState.AddTransition(_chaseState, _meleeAttackTimerCondition);

            Initialize(_idleState);
        }
    }
}