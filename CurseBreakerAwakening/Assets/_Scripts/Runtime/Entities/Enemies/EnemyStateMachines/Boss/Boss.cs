using CBA.Entities.Player.Weapons;
using CBA.Modules;
using DG.Tweening;
using GameCells.StateMachine;
using GameCells.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace CBA.Entities
{
    public class Boss : EnemyStateMachine
    {
        [SerializeField] private PhysicsQuery _playerDetector;
        [SerializeField] private PhysicsQuery _attackRangeDetector;
        [SerializeField] private GrabbableObject _grabbableObject;
        [SerializeField] private RagdollController _ragdollController;
        [SerializeField] private CombatAnimationEventHander _combatAnimationEventHander;

        [Header("Spells")]
        [SerializeField] private SpellManager _phase1Spells;
        [SerializeField] private SpellManager _phase2Spells;

        [Header(GameData.CUSTOMIZATION)]
        [SerializeField] private int _phaseCount = 2;
        [SerializeField] private float _minSpellInterval = 5f;
        [SerializeField] private float _maxSpellInterval = 8f;
        [SerializeField] private float _phase1EngagedMoveSpeed = 1f;
        [SerializeField] private float _phase2EngagedMoveSpeed = 2f;
        [SerializeField] private float _engageDistance = 3f;
        [SerializeField] private float _engageDistanceBias = 1f;
        public UnityEvent OnPhase2TransitionEvent;
        [SerializeField] private GameObject _roomTransitionDestroyVfx;


        #region States and Conditions
        private IdleState _idleState;
        private EngagedState _phase1EngagedState;
        private EngagedState _phase2EngagedState;
        private StunnedState _stunnedState;
        private GrabbedState _grabbedState;
        private RecoverState _recoverState;
        private BossPhaseTransitionState _bossPhase2TransitionState;
        private DeathState _deathState;

        //Spell states
        private List<SpellcastState> _phase1SpellcastStates = new List<SpellcastState>();
        private List<Condition> _phase1SpellAvailableConditions = new List<Condition>();
        private List<Condition> _phase1SpellCompletedConditions = new List<Condition>();

        private List<SpellcastState> _phase2SpellcastStates = new List<SpellcastState>();
        private List<Condition> _phase2SpellAvailableConditions = new List<Condition>();
        private List<Condition> _phase2SpellCompletedConditions = new List<Condition>();

        private Condition _playerInDetectionRangeCondition;
        private Condition _playerOutOfDetectionRangeCondition;
        private Condition _playerInAttackRangeCondition;
        private Condition _guardBrokenCondition;
        private Condition _stunTimerCondition;
        private Condition _grabbedCondition;
        private Condition _thrownTerrainCollisionCondition;
        private Condition _recoverAnimationFinishedCondition;
        private Condition _healthDepletedCondition;

        //Spells
        private Condition _spellIntervalCondition;

        //Boss specific
        private Condition_BossTransitionCompleted _bossTransitionCompletedCondition;
        private Condition_HasRemainingPhases _bossHasRemainingPhaseCondition;

        #endregion

        private void Awake()
        {
            //State Machine Initialization
            //1. State Initialization
            OnEnterPhase1();

            _idleState = new IdleState(entity, this);
            _phase1EngagedState = new EngagedState(entity, this, _engageDistance, _engageDistanceBias, _phase1EngagedMoveSpeed);
            _phase2EngagedState = new EngagedState(entity, this, _engageDistance, _engageDistanceBias, _phase2EngagedMoveSpeed);

            //Phase 1 Spells
            foreach (var entry in _phase1Spells.SpellDictionary)
            {
                Spell spell = entry.Value;

                SpellcastState spellcastState = new SpellcastState(entity, this, _phase1Spells, spell);
                _phase1SpellcastStates.Add(spellcastState);

                Condition spellAvailableCondition = new Condition_SpellAvailable(spell);
                _phase1SpellAvailableConditions.Add(spellAvailableCondition);

                Condition spellCompletedCondition = new Condition_SpellCastCompleted(spell);
                _phase1SpellCompletedConditions.Add(spellCompletedCondition);
            }

            //Phase 2 Spells
            foreach (var entry in _phase2Spells.SpellDictionary)
            {
                Spell spell = entry.Value;

                SpellcastState spellcastState = new SpellcastState(entity, this, _phase2Spells, spell);
                _phase2SpellcastStates.Add(spellcastState);

                Condition spellAvailableCondition = new Condition_SpellAvailable(spell);
                _phase2SpellAvailableConditions.Add(spellAvailableCondition);

                Condition spellCompletedCondition = new Condition_SpellCastCompleted(spell);
                _phase2SpellCompletedConditions.Add(spellCompletedCondition);
            }

            _bossPhase2TransitionState = new BossPhaseTransitionState(entity, this, _combatAnimationEventHander);

            _stunnedState = new StunnedState(entity, this, _grabbableObject);
            _grabbedState = new GrabbedState(entity, this, _grabbableObject);
            _recoverState = new RecoverState(entity, this, _grabbableObject);
            _deathState = new DeathState(entity, this, _ragdollController);

            //2. Condition Initialization
            _playerInDetectionRangeCondition = new Condition_PlayerInRange(_playerDetector);
            _playerOutOfDetectionRangeCondition = new Condition_PlayerOutOfRange(_playerDetector);
            _playerInAttackRangeCondition = new Condition_PlayerInRange(_attackRangeDetector);
            _guardBrokenCondition = new Condition_GuardBroken(this.ModuleManager.GetModule<GuardModule>());
            _stunTimerCondition = new Condition_Timer(entity.EntityData.BaseStunDuration);
            _grabbedCondition = new Condition_Grabbed(_grabbableObject);
            _thrownTerrainCollisionCondition = new Condition_ThrownTerrainCollision(_grabbableObject);
            _recoverAnimationFinishedCondition = new Condition_OnAnimationFinished(Animator);
            _healthDepletedCondition = new Condition_HealthDepleted(this.ModuleManager.GetModule<HealthModule>());

            _spellIntervalCondition = new Condition_Timer_Random(_minSpellInterval, _maxSpellInterval);

            _bossTransitionCompletedCondition = new Condition_BossTransitionCompleted(_combatAnimationEventHander);
            _bossHasRemainingPhaseCondition = new Condition_HasRemainingPhases(_phaseCount, this.GetModule<HealthModule>());

            //3. Setting up transitions
            _idleState.AddTransition(_phase1EngagedState, _playerInDetectionRangeCondition);

            #region Phase 1 Transitions
            for (int i = 0; i < _phase1SpellcastStates.Count; i++)
            {
                //Transition from engaged to cast
                _phase1EngagedState.AddTransition(_phase1SpellcastStates[i], new[] { _spellIntervalCondition, _phase1SpellAvailableConditions[i]});

                //Transition from cast back to engaged
                _phase1SpellcastStates[i].AddTransition(_phase1EngagedState, _phase1SpellCompletedConditions[i]);
            }
            #endregion

            #region Phase 2 Transitions
            for (int i = 0; i < _phase2SpellcastStates.Count; i++)
            {
                //Transition from engaged to cast
                _phase2EngagedState.AddTransition(_phase2SpellcastStates[i], new[] { _spellIntervalCondition, _phase2SpellAvailableConditions[i] });

                //Transition from cast back to engaged
                _phase2SpellcastStates[i].AddTransition(_phase2EngagedState, _phase2SpellCompletedConditions[i]);
            }
            #endregion


            _stunnedState.AddTransition(_idleState, _stunTimerCondition);
            _stunnedState.AddTransition(_grabbedState, _grabbedCondition);

            _grabbedState.AddTransition(_recoverState, _thrownTerrainCollisionCondition);

            _recoverState.AddTransition(_idleState, _recoverAnimationFinishedCondition);

            _bossPhase2TransitionState.AddTransition(_phase2EngagedState, _bossTransitionCompletedCondition);

            this.AddAnyTransition(_stunnedState, _guardBrokenCondition);
            this.AddAnyTransition(_bossPhase2TransitionState, new[] {_healthDepletedCondition, _bossHasRemainingPhaseCondition});
            this.AddAnyTransition(_deathState, _healthDepletedCondition);

            Initialize(_idleState);
        }

        private void OnEnable()
        {
            _bossPhase2TransitionState.OnTransitionStart += OnEnterPhase2;
            _bossPhase2TransitionState.OnTransitionEvent += Phase2TransitionEvent;
        }

        private void OnDisable()
        {
            _bossPhase2TransitionState.OnTransitionStart -= OnEnterPhase2;
            _bossPhase2TransitionState.OnTransitionEvent -= Phase2TransitionEvent;
        }

        private void Start()
        {
            OnEnterPhase1();
        }

        private void OnEnterPhase1()
        {
            _phase1Spells.SetActive(true);
            _phase2Spells.SetActive(false);
        }

        private void OnEnterPhase2()
        {
            _phase1Spells.SetActive(false);
            _phase2Spells.SetActive(true);
        }

        private void Phase2TransitionEvent()
        {
            OnPhase2TransitionEvent?.Invoke();
        }

        public async void TeleportToPosition(Transform teleportTransform)
        {
            await Task.Delay(100); //Wait 0.1 second

            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.enabled = false;

            transform.position = teleportTransform.position;
            transform.rotation = teleportTransform.rotation;

            DOVirtual.DelayedCall(1, () => navMeshAgent.enabled = true);
        }
    }
}