using Assets.Scripts.Game;
using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Extensions
{
    public static class TileTypeExtensions
    {
        public static Tile ToTile(this TileType tileType)
        {
            if (tileType != default)
            {
                var tile = new Tile()
                {
                    Reference = tileType.Reference,
                    IsDeadly = tileType.IsDeadly,
                    Material = GameFrame.Base.Resources.Manager.Materials.Get(tileType.Materials.GetRandomEntry())
                };

                if (tileType.ExtraTemplates?.Count > 0)
                {
                    tile.ExtraTemplate = tileType.ExtraTemplates.GetRandomEntry((t) => { return UnityEngine.Random.value > 0.95; })?.ToTile();
                }

                return tile;
            }

            return default;
        }
    }
}
