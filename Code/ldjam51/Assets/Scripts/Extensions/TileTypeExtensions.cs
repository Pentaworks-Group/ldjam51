using System.Collections.Generic;
using System.Linq;

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

                if (tileType.Extras?.Count > 0)
                {
                    if (UnityEngine.Random.value < Base.Core.SelectedGameMode.ExtraChance)
                    {
                        float take = UnityEngine.Random.value;
                        foreach (TileType t in Base.Core.SelectedGameMode.TileTypes.Extras)
                        {
                            float weight = Base.Core.SelectedGameMode.GetExtraWeights()[tileType.Reference].GetValueOrDefault(t.Reference, 0f);
                            if (weight > take)
                            {
                                tile.ExtraTemplate = t.ToTile();
                                break;
                            }
                        }

                    }
                }

                return tile;
            }

            return default;
        }
    }
}
