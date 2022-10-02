using System;

using Assets.Scripts.Model;

namespace Assets.Scripts.Game
{
    public class FieldState
    {
        public Boolean IsActive { get; set; }
        public Boolean IsPlaneVisible { get; set; }
        public Boolean IsCompleted { get; set; }
        public Int32 RowCount { get; set; }
        public Int32 ColumnCount { get; set; }
        public Player Player { get; set; }
        public Finish Finish { get; set; }
        public Tile[,] Tiles { get; set; }
    }
}
