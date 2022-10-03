using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core
{
    public class TileType
    {
        public String TemplateReference { get; set; }
        public List<String> Materials { get; set; }
        public Dictionary<String, float> Extras { get; set; }
    }
}
