using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core
{
    public class FinishType
    {
        public String TemplateReference { get; set; }
        public List<String> Materials { get; set; }
        public List<String> Extras { get; set; }
        public TileType Tile { get; set; }
    }
}
