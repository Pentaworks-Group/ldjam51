using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core
{
    public class GameState : GameFrame.Core.GameState
    {
        public GameSettings Mode { get; set; }
        public float ElapsedTime { get; set; }
        public String SaveGameName { get; set; }

    }
}
