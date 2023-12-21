using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.LevelGeneration
{
    public class Room : MonoBehaviour
    {
        [field: SerializeField] public ERoomShape RoomShape { get; private set; }
        [field: SerializeField] public Exit[] Exits { get; private set; }

        public event Action OnPlayerEnterRoom;
        public event Action<EExitDirection> OnPlayerExitRoom;

        private Dictionary<EExitDirection, Exit> _exitsDict = new Dictionary<EExitDirection, Exit>();

        public Exit GetEntrance(EExitDirection direction)
        {
            return _exitsDict[direction];
        }

        private void Awake()
        {
            foreach (var exit in Exits)
            {
                if (!_exitsDict.ContainsKey(exit.Direction))
                {
                    _exitsDict.Add(exit.Direction, exit);
                }
            }
        }

        private void OnEnable()
        {
            foreach (var exit in Exits) 
            {
                exit.OnPlayerExit += (EExitDirection direction) => OnPlayerExit(exit.Direction);
            }
        }

        private void OnDisable()
        {
            foreach (var exit in Exits)
            {
                exit.OnPlayerExit -= (EExitDirection direction) => OnPlayerExit(exit.Direction);
            }
        }

        public virtual void OnPlayerEnter() 
        {
            OnPlayerEnterRoom?.Invoke();
        }

        public virtual void OnPlayerExit(EExitDirection direction)
        {
            OnPlayerExitRoom?.Invoke(direction);
        }

        public void LockRoom()
        {
            foreach (var exit in this.Exits)
            {
                exit.Lock();
            }
        }

        public void UnlockRoom()
        {
            foreach (var exit in this.Exits)
            {
                exit.Unlock();
            }
        }
    }
}