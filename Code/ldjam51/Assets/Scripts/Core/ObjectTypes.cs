using System.Collections.Generic;

namespace Assets.Scripts.Core
{
    public class ObjectTypes
    {
        public List<PlayerType> Players { get; set; }
        public List<MonsterType> Monsters { get; set; }
        public List<FinishTileType> Finishes { get; set; }
        public List<TileType> Tiles { get; set; }
        public List<ExtraType> Extras { get; set; }
    }
}
