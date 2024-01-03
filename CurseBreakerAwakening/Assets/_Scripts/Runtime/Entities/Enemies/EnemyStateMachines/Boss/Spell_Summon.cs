using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class Spell_Summon : Spell
    {
        [SerializeField] private Transform[] _summonPoints;
        [SerializeField] private Entity[] _summonPrefabs;
        [SerializeField] private int _maxConcurrentSummons = 4;
        [SerializeField] private GameObject _summonVfx;

        public override bool IsAvailable => !this.isOnCooldown && (this._summons.Count < _maxConcurrentSummons);

        public override int SpellAnimationHash => GameData.CASTSUMMON_HASH;

        private List<Entity> _summons = new List<Entity>();

        private List<Entity> _currentSummons = new List<Entity>();

        public override void Cast()
        {
            base.Cast();

            _currentSummons.Clear();

            for (int i = 0; i < _summonPoints.Length; i++)
            {
                if (_summons.Count >= _maxConcurrentSummons)
                    break; 

                if (_summonVfx != null)
                {
                    Instantiate(_summonVfx, _summonPoints[i].position, Quaternion.identity);
                }

                var summon = Instantiate(_summonPrefabs[Random.Range(0, _summonPrefabs.Length)], _summonPoints[i].position, Quaternion.identity);
                summon.OnDeath.AddListener(OnSummonDied);
                summon.gameObject.SetActive(false);
                _summons.Add(summon);
                _currentSummons.Add(summon);
            }
        }

        private void OnSummonDied(Entity summon)
        {
            _summons.Remove(summon);
            summon.OnDeath.RemoveListener(OnSummonDied);
        }

        public override void Complete()
        {
            base.Complete();

            for (int i = 0; i < _currentSummons.Count; i++)
            {
                _currentSummons[i].gameObject.SetActive(true);
            }
        }
    }
}