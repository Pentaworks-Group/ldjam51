using System;

namespace Assets.Scripts.Core
{
    public class PlayerOptions : GameFrame.Core.PlayerOptions
    {
        public String MobileInterface { get; set; } = "Right";
        public bool ShowTutorial { get; set; } = true;

    }
}
