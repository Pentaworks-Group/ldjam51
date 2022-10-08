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

        [Obsolete("Legacy: The setter will only set the ActiveFieldIndex.")]
        public Int32 ToggleIndex
        {
            set
            {
                this.ActiveFieldIndex = value;
            }
        }

        [Obsolete("Legacy: The setter will add the field to the Fields list.")]
        public FieldState Field1
        {
            set
            {
                if (value != default)
                {
                    if (this.Fields == default)
                    {
                        this.Fields = new List<FieldState>();
                    }

                    this.Fields.Add(value);
                }
            }
        }

        [Obsolete("Legacy: The setter will add the field to the Fields list.")]
        public FieldState Field2
        {
            set
            {
                if (value != default)
                {
                    if (this.Fields == default)
                    {
                        this.Fields = new List<FieldState>();
                    }

                    this.Fields.Add(value);
                }
            }
        }
    }
}
