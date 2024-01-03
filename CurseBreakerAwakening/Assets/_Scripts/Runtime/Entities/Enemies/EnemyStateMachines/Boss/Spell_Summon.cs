using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class Spell_Summon : Spell
    {
        [SerializeField] private Transform[] _summonPoints;
        [SerializeField] private Entity[] _summonPrefabs;

        public override int SpellAnimationHash => GameData.CASTSUMMON_HASH;

        private List<Entity> _summons = new List<Entity>();

        public override void Cast()
        {
            base.Cast();

            for (int i = 0; i < _summonPoints.Length; i++)
            {
                _summons.Add(Instantiate(_summonPrefabs[Random.Range(0, _summonPrefabs.Length)], _summonPoints[i].position, Quaternion.identity));
            }
        }
    }
}