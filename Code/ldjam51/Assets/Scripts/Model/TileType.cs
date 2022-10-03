using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class TileType
    {
        public String Reference { get; set; }
        public Boolean IsDeadly { get; set; }
        public List<String> Materials { get; set; }
        public Dictionary<String, float> Extras { get; set; }
        public List<String> SoundEffects { get; set; }
    }
}
