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

        private void Awake()
        {
            SpellDictionary = new Dictionary<int, Spell>();

            var spells = GetComponentsInChildren<Spell>();

            for (int i = 0; i < spells.Length; i++)
            {
                SpellDictionary.Add(i, spells[i]);
                Debug.Log($"Spell {i}: {spells[i].name}");
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

        public void SetCurrentSpell(Spell spell)
        {
            _currentSpell = spell;
        }

        public void CastSpell()
        {
            if (_currentSpell == null)
            {
                Debug.LogError("No spell assigned but CastSpell is called!");
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
            if (_currentSpell == null)
            {
                Debug.LogError("No spell assigned but CompleteSpell is called!");
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