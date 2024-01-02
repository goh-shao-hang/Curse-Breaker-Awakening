using CBA.Entities.Player.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class SpellManager : MonoBehaviour
    {
        [SerializeField] private CombatAnimationEventHander _combatAnimationEventHander;
        private Dictionary<int, Spell> _spellDictionary;

        private void Awake()
        {
            _spellDictionary = new Dictionary<int, Spell>();

            var spells = GetComponentsInChildren<Spell>();

            for (int i = 0; i < spells.Length; i++)
            {
                _spellDictionary.Add(i, spells[i]);
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

        public void CastSpell(int index)
        {
            if (!_spellDictionary.ContainsKey(index))
            {
                Debug.LogError($"Spell Index {index} Not Found!");
                return;
            }

            _spellDictionary[index].Cast();
        }

        public void CompleteSpell(int index)
        {
            if (!_spellDictionary.ContainsKey(index))
            {
                Debug.LogError($"Spell Index {index} Not Found!");
                return;
            }

            _spellDictionary[index].Complete();
        }
    }
}