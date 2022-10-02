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
                    TemplateReference = tileType.TemplateReference,
                    Material = GameFrame.Base.Resources.Manager.Materials.Get(tileType.Materials.GetRandomEntry())
                };

                if (tileType.ExtraTemplateReference?.Count > 0)
                {
                    tile.ExtraTemplateReference = tileType.ExtraTemplateReference.GetRandomEntry((s) => { return UnityEngine.Random.value > 0.95; });
                }

                return tile;
            }

            return default;
        }
    }
}
