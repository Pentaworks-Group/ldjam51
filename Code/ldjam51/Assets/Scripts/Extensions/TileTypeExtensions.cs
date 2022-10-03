using System.Linq;

using Assets.Scripts.Game;
using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Extensions
{
    public static class TileTypeExtensions
    {
        public static Tile ToTile(this TileType tileType, GameSettings gameMode)
        {
            if (tileType != default)
            {
                var tile = new Tile()
                {
                    Reference = tileType.Reference,
                    IsDeadly = tileType.IsDeadly,
                    Material = GameFrame.Base.Resources.Manager.Materials.Get(tileType.Materials.GetRandomEntry())
                };

                if (tileType.Extras?.Count > 0)
                {
                    var randomExtra = tileType.Extras.GetRandomEntry();

                    if (randomExtra.HasValue)
                    {
                        var extraTileType = gameMode.TileTypes.Extras.FirstOrDefault(t => t.Reference == randomExtra.Value.Key);

                        tile.ExtraTemplate = extraTileType.ToTile(gameMode);
                    }
                }

                return tile;
            }

            return default;
        }
    }
}
