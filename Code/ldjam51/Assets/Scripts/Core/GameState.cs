using System;
using System.Collections.Generic;

using Assets.Scripts.Game;

namespace Assets.Scripts.Core
{
    public class GameState : GameFrame.Core.GameState
    {
        public GameSettings Mode { get; set; }
        public float ElapsedTime { get; set; }
        public String SaveGameName { get; set; }
        public FieldState Field1 { get; set; }
        public FieldState Field2 { get; set; }
        public Single TimeRemaining { get; set; }
        public Int32 LevelsCompleted { get; set; }
    }
}
