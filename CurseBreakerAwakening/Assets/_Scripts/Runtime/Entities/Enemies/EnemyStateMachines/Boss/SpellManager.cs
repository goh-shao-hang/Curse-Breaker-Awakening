using CBA.Entities.Player.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class SpellManager : MonoBehaviour
    {
        [SerializeField] private CombatAnimationEventHander _combatAnimationEventHander;
        public Dictionary<int, Spell> SpellDictionary { get; private set; }

        private Spell _currentSpell = null;


        public bool IsActive { get; private set; } = true;

        private void Awake()
        {
            SpellDictionary = new Dictionary<int, Spell>();

            var spells = GetComponentsInChildren<Spell>(false);

            for (int i = 0; i < spells.Length; i++)
            {
                SpellDictionary.Add(i, spells[i]);
            }
        }

        private void OnEnable()
        {
            _combatAnimationEventHander.OnSpellCast += CastSpell;
            _combatAnimationEventHander.OnSpellComplete += CompleteSpell;
        }

        private void OnDisable()
        {
            _combatAnimationEventHander.OnSpellCast -= CastSpell;
            _combatAnimationEventHander.OnSpellComplete -= CompleteSpell;
        }

        public void SetActive(bool active)
        {
            this.IsActive = active;
        }

        public void SetCurrentSpell(Spell spell)
        {
            _currentSpell = spell;
        }

        public void CastSpell()
        {
            if (!IsActive)
                return;

            if (_currentSpell == null)
            {
                Debug.LogWarning("No spell assigned but CastSpell is called!");
                return;
            }

            _currentSpell.Cast();

           /* if (!SpellDictionary.ContainsKey(index))
            {
                Debug.LogError($"Spell Index {index} Not Found!");
                return;
            }

            SpellDictionary[index].Cast();*/
        }

        public void CompleteSpell()
        {
            if (!IsActive)
                return;

            if (_currentSpell == null)
            {
                Debug.LogWarning("No spell assigned but CompleteSpell is called!");
                return;
            }

            _currentSpell.Complete();

            /*if (!SpellDictionary.ContainsKey(index))
            {
                Debug.LogError($"Spell Index {index} Not Found!");
                return;
            }

            SpellDictionary[index].Complete();*/
        }
    }
}