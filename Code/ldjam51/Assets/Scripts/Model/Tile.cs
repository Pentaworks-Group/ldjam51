using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Tile
    {
        public String Reference { get; set; }
        public Tile ExtraTemplate { get; set; }
        public Material Material { get; set; }
    }
}
