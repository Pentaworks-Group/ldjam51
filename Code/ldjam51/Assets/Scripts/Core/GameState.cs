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
        public Single TimeRemaining { get; set; }
        public Int32 LevelsCompleted { get; set; } = 0;
        public Int32 ActiveFieldIndex { get; set; } = -1;
        public float NextTick { get; set; }
        public Int32 LastTick { get; set; }
        public List<FieldState> Fields { get; set; }
        public String DeathReason { get; set; }
        public String WatchOutForText { get; set; }
    }
}
