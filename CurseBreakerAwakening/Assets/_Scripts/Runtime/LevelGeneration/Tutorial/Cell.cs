using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CBA.LevelGeneration
{
    public class Cell
    {
        public bool Visited = false;
        public bool[] Exits = new bool[4]; //Up -> Right -> Down -> Left

        public ERoomShape RoomShape { get; private set; }
        public float RoomRotation { get; private set; }

        public void UpdateTypeAndRotation()
        {
            int exitCount = 0;
            for (int i = 0; i < Exits.Length; i++)
            {
                if (Exits[i])
                {
                    exitCount++;
                }
            }

            if (exitCount == 4)
            {
                RoomShape = ERoomShape.FourExits;
                RoomRotation = 0;
            }
            else if (exitCount == 3)
            {
                RoomShape = ERoomShape.ThreeExits;

                int closedExitIndex = 0;
                for (int i = 0; i < Exits.Length; i++)
                {
                    if (!Exits[i])
                    {
                        break;
                    }
                    closedExitIndex++;
                }

                RoomRotation = 180 + (90 * closedExitIndex); //Face away from closed exit
            }
            else if (exitCount == 2)
            {
                if (Exits[0] == Exits[2])
                {
                    RoomShape = ERoomShape.TwoExits_I;
                    if (Exits[0])
                    {
                        RoomRotation = 0f;
                    }
                    else
                    {
                        RoomRotation = 90f;
                    }
                }
                else
                {
                    RoomShape = ERoomShape.TwoExits_L;

                    switch (true)
                    {
                        case true when Exits[0] && Exits[1]:
                            RoomRotation = 0f;
                            break;
                        case true when Exits[1] && Exits[2]:
                            RoomRotation = 90f;
                            break;
                        case true when Exits[2] && Exits[3]:
                            RoomRotation = 180f;
                            break;
                        case true when Exits[3] && Exits[0]:
                            RoomRotation = 270f;
                            break;
                    }
                }
            }
            else if (exitCount == 1)
            {
                RoomShape = ERoomShape.SingleExit;

                int openExitIndex = 0;
                for (int i = 0; i < Exits.Length; i++)
                {
                    if (Exits[i])
                    {
                        break;
                    }
                    openExitIndex++;
                }

                RoomRotation = 90 * openExitIndex;
            }
        }
    }
}