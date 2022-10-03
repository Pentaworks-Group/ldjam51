using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class TileTypes
    {
        public List<TileType> Players { get; set; }
        public List<TileType> Monsters { get; set; }
        public List<TileType> Finishes { get; set; }
        public List<TileType> Tiles { get; set; }
        public List<TileType> Extras { get; set; }
    }
}
