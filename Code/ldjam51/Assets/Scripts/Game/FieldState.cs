using System;
using System.Collections.Generic;

using Assets.Scripts.Model;

namespace Assets.Scripts.Game
{
    public class FieldState
    {
        public Boolean IsActive { get; set; }
        public Boolean IsPlaneVisible { get; set; }
        public Boolean IsCompleted { get; set; }
        public Int32 Index { get; set; }
        public Int32 RowCount { get; set; }
        public Int32 ColumnCount { get; set; }
        public Player Player { get; set; }
        public Finish Finish { get; set; }
        public List<Monster> Monsters { get; set; }
        public Tile[,] Tiles { get; set; }
    }
}
