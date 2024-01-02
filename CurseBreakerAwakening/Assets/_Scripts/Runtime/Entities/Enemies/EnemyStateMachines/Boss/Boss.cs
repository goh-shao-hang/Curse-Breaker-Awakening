using GameCells.StateMachine;
using GameCells.Utilities;
using UnityEngine;

namespace CBA.Entities
{
    public class Boss : EnemyStateMachine
    {
        [SerializeField] private PhysicsQuery _playerDetector;
        [SerializeField] private PhysicsQuery _attackRangeDetector;
        [SerializeField] private GrabbableObject _grabbableObject;
        [SerializeField] private RagdollController _ragdollController;

        [Header("Spells")]
        [SerializeField] private Spell_Shield _shield;
        [SerializeField] private float _shieldCastTimer = 2f;
        [SerializeField] private Spell_Multishot _multishot;
        [SerializeField] private float _multishotCastTimer = 2f;

        [Header(GameData.CUSTOMIZATION)]
        [SerializeField] private float _engagedMoveSpeed = 1f;
        [SerializeField] private float _engageDistance = 3f;
        [SerializeField] private float _meleeAttackDuration = 1f;

        #region States and Conditions
        private IdleState _idleState;
        private SpellcastState _shieldSpellState;
        private SpellcastState _multishotSpellState;
        private EngagedState _phase1EngagedState;
        //private MeleeAttackState _meleeAttackState;
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

        //Spells
        private Condition_SpellAvailable _shieldAvailableCondition;
        private Condition_Timer _shieldCastTimerCondition;
        private Condition_SpellAvailable _multishotAvailableCondition;
        private Condition_Timer _multishotCastTimerCondition;
        #endregion

        private void Awake()
        {
            //State Machine Initialization
            //1. State Initialization
            _idleState = new IdleState(entity, this);
            _phase1EngagedState = new EngagedState(entity, this, _engageDistance, _engagedMoveSpeed);
            _shieldSpellState = new SpellcastState(entity, this, _shield, GameData.CASTSHIELD_HASH);
            _multishotSpellState = new SpellcastState(entity, this, _multishot, GameData.CASTMULTISHOT_HASH);
            //_meleeAttackState = new MeleeAttackState(entity, this, meleeAttack);
            _stunnedState = new StunnedState(entity, this, _grabbableObject);
            _grabbedState = new GrabbedState(entity, this, _grabbableObject);
            _recoverState = new RecoverState(entity, this);
            _deathState = new DeathState(entity, this, _ragdollController);

            //2. Condition Initialization
            _playerInDetectionRangeCondition = new Condition_PlayerInRange(_playerDetector);
            _playerOutOfDetectionRangeCondition = new Condition_PlayerOutOfRange(_playerDetector);
            _playerInAttackRangeCondition = new Condition_PlayerInRange(_attackRangeDetector);
            _meleeAttackTimerCondition = new Condition_Timer(_meleeAttackDuration);
            _guardBrokenCondition = new Condition_GuardBroken(this.ModuleManager.GetModule<GuardModule>());
            _stunTimerCondition = new Condition_Timer(entity.EntityData.BaseStunDuration);
            _grabbedCondition = new Condition_Grabbed(_grabbableObject);
            _thrownTerrainCollisionCondition = new Condition_ThrownTerrainCollision(_grabbableObject);
            _recoverAnimationFinishedCondition = new Condition_OnAnimationFinished(Animator);
            _healthDepletedCondition = new Condition_HealthDepleted(this.ModuleManager.GetModule<HealthModule>());

            _shieldAvailableCondition = new Condition_SpellAvailable(_shield);
            _shieldCastTimerCondition = new Condition_Timer(_shieldCastTimer);
            _multishotAvailableCondition = new Condition_SpellAvailable(_multishot);
            _multishotCastTimerCondition = new Condition_Timer(_multishotCastTimer);

            //3. Setting up transitions
            //_idleState.AddTransition(_shieldState , new[] { _shieldAvailableCondition, _playerInDetectionRangeCondition});
            _idleState.AddTransition(_phase1EngagedState, _playerInDetectionRangeCondition);

            _phase1EngagedState.AddTransition(_shieldSpellState, _shieldAvailableCondition);
            _phase1EngagedState.AddTransition(_stunnedState, _multishotAvailableCondition);

            _shieldSpellState.AddTransition(_phase1EngagedState, _shieldCastTimerCondition);
            //_multishotSpellState.AddTransition(_phase1EngagedState, _multishotCastTimerCondition);

            //_chaseState.AddTransition(_idleState, _playerOutOfDetectionRangeCondition);
            //_chaseState.AddTransition(_meleeAttackState, _playerInAttackRangeCondition);

            //_meleeAttackState.AddTransition(_chaseState, _meleeAttackTimerCondition);

            _stunnedState.AddTransition(_idleState, _stunTimerCondition);
            _stunnedState.AddTransition(_grabbedState, _grabbedCondition);

            _grabbedState.AddTransition(_recoverState, _thrownTerrainCollisionCondition);

            _recoverState.AddTransition(_idleState, _recoverAnimationFinishedCondition);

            this.AddAnyTransition(_stunnedState, _guardBrokenCondition);
            this.AddAnyTransition(_deathState, _healthDepletedCondition);

            Initialize(_idleState);
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}