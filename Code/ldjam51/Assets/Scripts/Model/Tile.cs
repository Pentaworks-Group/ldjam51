using System;

using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Tile
    {
        public String Reference { get; set; }
        public Boolean IsStart { get; set; }
        public Boolean IsFinish { get; set; }
        public Boolean IsDeadly { get; set; }
        public Tile ExtraTemplate { get; set; }
        public Material Material { get; set; }
    }
}
